using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for Sign_Up.xaml
    /// </summary>
    public partial class Sign_Up : Page
    {
        public Sign_Up()
        {
            InitializeComponent();
            KeySizeBox.Items.Add(512);
            KeySizeBox.Items.Add(1024);
            KeySizeBox.Items.Add(2048);
            KeySizeBox.Items.Add(3072);
            KeySizeBox.Items.Add(4096);
        }

        private async void Sign_Up_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch keyGenTime = new Stopwatch();
            if (!string.IsNullOrWhiteSpace(Name.Text) && !string.IsNullOrWhiteSpace(Username.Text) && !string.IsNullOrWhiteSpace(Password.Password) && !string.IsNullOrWhiteSpace(ConfirmPassword.Password))
            {
                if (string.Equals(Password.Password, ConfirmPassword.Password))
                {
                    int SelectedKeySize = int.Parse(KeySizeBox.Text);
                    MyRSAProvider.MyMyKeys rsa = new MyRSAProvider.MyMyKeys();
                    keyGenTime.Reset();
                    keyGenTime.Start();
                        rsa = RSAMain.GenerateKey(SelectedKeySize);
                    keyGenTime.Stop();
                    KeyGenTime.Content = keyGenTime.ElapsedMilliseconds.ToString();
                        var newPublicKey = rsa.PublicKey.FirstOrDefault();
                        var newPrivateKey = rsa.PrivateKey.FirstOrDefault();
                        string publicKey = newPublicKey.E;
                        string privateKey = newPrivateKey.D;
                        string modulus = newPrivateKey.N;
                    string Dp = newPrivateKey.Dp;
                    string Dq = newPrivateKey.Dq;
                    string Qinv = newPrivateKey.Qinv;
                    string P = newPrivateKey.P;
                    string Q = newPrivateKey.Q;
                    string Ep = newPrivateKey.Dp;
                    string Eq = newPrivateKey.Dq;
                    
                    string Id = DateTime.Now.ToString();
                        RSA2017TestDBContext context = new RSA2017TestDBContext();
                        var CheckUsername = context.RSA2017TestDB.Where(m => m.Username == Username.Text).FirstOrDefault();
                    MessageBox.Show("User has been created and keys have been generated for the user.\nClick OK to be redirected to landing page.");

                    if (CheckUsername == null)
                        {
                            var UserKey = context.MyKeys.Add(new MyKeys { KeyId = Id, D  = privateKey, E = publicKey, KeySize = SelectedKeySize, N = modulus, Dp = Dp, Dq = Dq, Ep = Ep, Eq = Eq, P = P, Q = Q, Qinv = Qinv});
                            var NewUser = context.RSA2017TestDB.Add(new RSA2017TestDB { KeyId = Id, Name = Name.Text, PassWord = Password.Password, UserId = Id, Username = Username.Text, MyKeys = UserKey, Email = EmailTxt.Text });
                            await context.SaveChangesAsync();
                            this.NavigationService.Navigate(new RSAIndex(NewUser.UserId));
                        }
                        else
                        {
                            ErrorMsg.Content = "Username already exist.";
                            ErrorMsg.Visibility = Visibility.Visible;
                        }

                }
                else
                {
                    ErrorMsg.Content = "Password and Confirm Password doesn't match.";
                    ErrorMsg.Visibility = Visibility.Visible;
                }
            }
            else
            {
                ErrorMsg.Content = "Enter valid details and try again.";
                ErrorMsg.Visibility = Visibility.Visible;
            }
       
        }
            


        


    }
}
