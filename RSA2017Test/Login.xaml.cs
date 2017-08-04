using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            RSA2017TestDBContext context = new RSA2017TestDBContext();
            var Verify = context.RSA2017TestDB.Where(m => m.Username == Username.Text).FirstOrDefault();
            if (Verify != null)
            {
                if (Verify.PassWord == Password.Password)
                {
                    this.NavigationService.Navigate(new RSAIndex(Verify.UserId));
                    // Uri uri = new Uri("RSAIndex.xaml", UriKind.Relative);
                    //this.NavigationService.Navigate(uri);
                }
                else
                {
                    ErrorMsg.Content = "Password incorrect";
                    ErrorMsg.Visibility = Visibility.Visible;
                }
            }
            else
            {
                ErrorMsg.Content = "Username incorrect";
                ErrorMsg.Visibility = Visibility.Visible;
            }

        }


        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("Sign Up.xaml", UriKind.Relative);
            this.NavigationService.Navigate(uri);
        }
    }
}
