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
    /// Interaction logic for PlainText.xaml
    /// </summary>
    public partial class PlainText : Page
    {
        public string GottenId;
        public PlainText(string id)
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
            Stopwatch timer = new Stopwatch();
            Timerlbl.Content = timer.ElapsedMilliseconds.ToString();
            RSA2017TestDBContext context = new RSA2017TestDBContext();

            var currentUser = context.RSA2017TestDB.Where(m => m.Username == SelectUser.Text).FirstOrDefault();
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
                string[] Input = PlainTxtBlock.Text.Split(' ');
                string[] output = new string[Input.Length];
                byte[] dataToEncrypt = new byte[Size];
                timer.Reset();
                for (int i = 0; i <= (Input.Length - 1); i++)
                {
                    
                    
                    string Encrypted = string.Empty;
                    if (Input[i] == string.Empty)
                    {
                        Input[i] = Input[i] + " ";
                        
                    }
                    timer.Start();
                    byte[] data = Encoding.UTF8.GetBytes(Input[i]);
                   byte[] use = RSAMain.Encrypt(data,Ep,Eq,Qinv,P,Q,Size,E,N);
                    
                    Encrypted = Convert.ToBase64String(use);
                    output[i] = Encrypted;
                }
                EncryptedTxtBlock.Text = string.Join("$", output);
                Timerlbl.Content = timer.ElapsedMilliseconds.ToString();
                timer.Stop();
                Timerlbl.Content = timer.ElapsedMilliseconds.ToString();
                Savebtn.IsEnabled = true;
            }
            catch (Exception)
            {

                MessageBox.Show("Select a receiver");
            }

        }
    }
}
