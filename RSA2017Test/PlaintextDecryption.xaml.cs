using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for PlaintextDecryption.xaml
    /// </summary>
    public partial class PlaintextDecryption : Page
    {
        public string GottenId;
        public PlaintextDecryption(string id)
        {
            
            InitializeComponent();
            RSA2017TestDBContext context = new RSA2017TestDBContext();
            foreach (var item in context.RSA2017TestDB)
            {
                SelectUser.Items.Add(item.Username);
            }
            GottenId = id;
        }

        private string[] textArray = new string[1000000]; //create an instance of textArray so it can be used multiple times and accessible in all functions

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Encrptbtn.IsEnabled = true;
            //create an array of strings 
            var listOfStrings = new List<string>();
            textArray = listOfStrings.ToArray();

            Microsoft.Win32.OpenFileDialog open1 = new Microsoft.Win32.OpenFileDialog();
            // open1.Filter = "TEXT|*.txt";
            if (open1.ShowDialog().HasValue)
            {
                Filetxtbox.Text = open1.FileName;
                Encrptbtn.IsEnabled = true;
                string file = open1.FileName; //saves the input file from the openFiledialog window 
                try
                {
                    string text = File.ReadAllText(file); //convert the input file to a set of strings

                    textArray = text.Split(' '); //converts the set of strings in arrays delimited by whitespaces
                    PlainTxtBlock.Text = text;
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
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files(*.txt)|*.txt|All(*.*)|*|Word(*docx)|*docx";
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, EncryptedTxtBlock.Text);
        }

        private void Encrptbtn_Click(object sender, RoutedEventArgs e)
        {
            RSA2017TestDBContext context = new RSA2017TestDBContext();
            Stopwatch timer = new Stopwatch();
            Timerlbl.Content = timer.ElapsedMilliseconds.ToString();
            var currentUser = context.RSA2017TestDB.Where(m => m.Username == SelectUser.Text).FirstOrDefault();
            try
            {
                BigInteger Dp = BigInteger.Parse(currentUser.MyKeys.Dp);
                BigInteger Dq = BigInteger.Parse(currentUser.MyKeys.Dq);
                BigInteger Qinv = BigInteger.Parse(currentUser.MyKeys.Qinv);
                BigInteger P = BigInteger.Parse(currentUser.MyKeys.P);
                BigInteger Q = BigInteger.Parse(currentUser.MyKeys.Q);
                BigInteger D = BigInteger.Parse(currentUser.MyKeys.D);
                BigInteger n = BigInteger.Parse(currentUser.MyKeys.N);
                int Size = currentUser.MyKeys.KeySize;
                string[] Input = PlainTxtBlock.Text.Split('$');
                string[] output = new string[Input.Length];
                byte[] dataToEncrypt = new byte[Size];
                timer.Reset();
                for (int i = 0; i <= (Input.Length - 1); i++)
                {
                    byte[] data = new byte[Size];
                    byte[] use = new byte[Size];
                    string Decrypted = string.Empty;
                    if (Input[i] == string.Empty)
                    {
                        Input[i] = Input[i] + " ";

                    }
                    timer.Start();
                    data = Convert.FromBase64String(Input[i]);
                    use = RSAMain.Decrypt(data,Dp,Dq,Qinv,P,Q,Size,D,n);
                    Decrypted = Encoding.UTF8.GetString(use);
                    output[i] = Decrypted;
                    Timerlbl.Content = timer.ElapsedMilliseconds.ToString();
                }
                EncryptedTxtBlock.Text = string.Join(" ", output);
                timer.Stop();
                Timerlbl.Content = timer.ElapsedMilliseconds.ToString();
                Savebtn.IsEnabled = true;
            }
            catch (Exception)
            {

                MessageBox.Show("Decryption Failed!");
            }

        }
    }
}
