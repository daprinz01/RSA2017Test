using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string helpurl1 = ((Directory.GetCurrentDirectory() + "\\Images\\free-encryption.jpg"));
            //System.Diagnostics.Process.Start(helpurl1);
            ImageSource imageSource = new BitmapImage(new Uri(helpurl1));
            backgroundImage.ImageSource = imageSource;
            Mainframe.Navigate(new Login());
        }
    }
}
