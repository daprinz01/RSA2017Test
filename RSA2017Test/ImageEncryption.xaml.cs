using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RSA2017Test
{
    /// <summary>
    /// Interaction logic for ImageEncryption.xaml
    /// </summary>
    public partial class ImageEncryption : Page
    {
        string GottenId { get; set; }

        public ImageEncryption(string id)
        {
            GottenId = id;
            InitializeComponent();
            RSA2017TestDBContext context = new RSA2017TestDBContext();
            foreach (var item in context.RSA2017TestDB)
            {
                SelectUser.Items.Add(item.Username);
            }
        }
        BitmapSource imageSource { get; set; }
        byte[] Imgbytes { get; set; }
        private string[] textArray = new string[1000000]; //create an instance of textArray so it can be used multiple times and accessible in all functions

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Encrptbtn.IsEnabled = true;
            //create an array of strings 
            var listOfStrings = new List<string>();
            textArray = listOfStrings.ToArray();

            Microsoft.Win32.OpenFileDialog open1 = new Microsoft.Win32.OpenFileDialog();
            open1.Filter = "JPG|*.JPG";
            if (open1.ShowDialog().HasValue)
            {
                Filetxtbox.Text = open1.FileName;
                Encrptbtn.IsEnabled = true;
                string file = open1.FileName; //saves the input file from the openFiledialog window 
                try
                {
                    Imgbytes = File.ReadAllBytes(file);
                     imageSource = new BitmapImage(new Uri(file));
                    OriginalImg.Source = imageSource;
                    //Imgppt = File.GetAttributes(file);

                    //string text = File.ReadAllText(file); //convert the input file to a set of strings

                    //textArray = text.Split(' '); //converts the set of strings in arrays delimited by whitespaces
                    //PlainTxtBlock.Text = text;
                    // arraySize.Text = (textArray.Length - 1).ToString();
                    // label5.Visible = true;
                    // arraySize.Visible = true;
                }
                //throw exception in case of an error
                catch (Exception)
                {

                    System.Windows.MessageBox.Show("Choose a valid file type");

                }
            }
            else
            {
                Filetxtbox.Text = "";
                Encrptbtn.IsEnabled = false;
            }
        }

        private void Savebtn_Click(object sender, RoutedEventArgs e)
        {
            /*    SaveFileDialog sfd = new SaveFileDialog();
               sfd.Filter = "Images|*.png;*.bmp;*.jpg";
               ImageFormat format = ImageFormat.Png;

              if (sfd.ShowDialog().HasValue)
               {
                   string ext = System.IO.Path.GetExtension(sfd.FileName);
                   switch (ext)
                   {
                       case ".jpg":
                           format = ImageFormat.Jpeg;
                           break;
                       case ".bmp":
                           format = ImageFormat.Bmp;
                           break;
                   }
                   EncryptedImg.Image.Save(sfd.FileName, format);
               }



               SaveFileDialog saveFileDialog = new SaveFileDialog();
               if (saveFileDialog.ShowDialog() == true)
                   File.(saveFileDialog.FileName, EncryptedImg.Source);*/
        }
        //method to convert the encrypted bytes to a viewable image object
        private static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        private void Encrptbtn_Click(object sender, RoutedEventArgs e)
        {
            RSA2017TestDBContext context = new RSA2017TestDBContext();
            BitmapImage EncryptedImage = new BitmapImage();
            
            var currentUser = context.RSA2017TestDB.Where(m => m.Username == SelectUser.Text).FirstOrDefault();
            int l = 0;
            try
            {
                BigInteger Ep = BigInteger.Parse(currentUser.MyKeys.Ep);
                BigInteger Eq = BigInteger.Parse(currentUser.MyKeys.Eq);
                BigInteger Qinv = BigInteger.Parse(currentUser.MyKeys.Qinv);
                BigInteger P = BigInteger.Parse(currentUser.MyKeys.P);
                BigInteger Q = BigInteger.Parse(currentUser.MyKeys.Q);
                BigInteger E = BigInteger.Parse(currentUser.MyKeys.E);
                BigInteger N = BigInteger.Parse(currentUser.MyKeys.N);
                int Size = currentUser.MyKeys.KeySize;
                byte[] EncryptedBytes;
                byte[] NewImg = new byte[(34*Size) * (Imgbytes.Length/100)];
                byte[] Encrypted = new byte[Imgbytes.Length];
                int count = 0;
                for (int i = 0; i < Imgbytes.Length;)
                {
                    byte[] bitsOfBytes = new byte[100];
                    for (int j = 0; j < 100; j++)
                    {
                        l = i + j;
                        if (l == (Imgbytes.Length - 1))
                            break;
                        bitsOfBytes[j] = Imgbytes[l];

                    }
                    try
                    {
                        EncryptedBytes = RSAMain.Encrypt(bitsOfBytes, Ep, Eq, Qinv, P, Q, Size, E, N);

                        //Image encryptedImg = BitmapImage.Create(Imgppt.)
                        for (int m = 0; m < EncryptedBytes.Length; m++)
                        {
               
                            NewImg[m + count] = EncryptedBytes[m];
                            Encrypted[m] = EncryptedBytes[m];
                        }
                        count += EncryptedBytes.Length;
                        try
                        {
                            BitmapSource bitmap = BitmapSource.Create(2, 2, imageSource.Width, imageSource.Height, PixelFormats.Indexed8, BitmapPalettes.WebPalette, Encrypted, 2);

                            EncryptedImg.Source = bitmap;
                            // EncryptedImage = LoadImage(Encrypted);
                            //EncryptedImg.Source = EncryptedImage;
                        }
                        catch (Exception exp)
                        {

                            MessageBox.Show("Error in converting image\n" + exp.ToString());
                        }

                        
                    }
                    catch (Exception imgexp)
                    {

                        MessageBox.Show("Encryption Failed!\nTry again with reduced file size" + imgexp);
                    }
                    if (l == (Imgbytes.Length - 1))
                        break;
                    i = i + 100;
                    
                }
                BitmapSource bitmapSource = BitmapSource.Create(2, 2, imageSource.Width, imageSource.Height, PixelFormats.Indexed8, BitmapPalettes.Gray256, NewImg, 2);

                EncryptedImg.Source = bitmapSource;
            }
            catch (Exception)
            {

                MessageBox.Show("Select a receiver");
            }
        }
    }
}

