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
    /// Interaction logic for RSAIndex.xaml
    /// </summary>
    public partial class RSAIndex : Page
    {
        public string gottenId { get; set; }
        public RSAIndex(string Id)
        {
            gottenId = Id;

            InitializeComponent();
            EncryptionFrame.Navigate(new PlainText(gottenId));
        }
        private void AddPresetButton_Click(object sender, RoutedEventArgs e)
        {
            var addButton = sender as FrameworkElement;
            if (addButton != null)
            {
                addButton.ContextMenu.IsOpen = true;
            }
        }



        private void Plaintext_Click(object sender, RoutedEventArgs e)
        {
            EncryptionFrame.Navigate(new PlainText(gottenId));
        }

        private void Image_Click(object sender, RoutedEventArgs e)
        {
            EncryptionFrame.Navigate(new ImageEncryption(gottenId));
        }

        private void PlaintextD_Click(object sender, RoutedEventArgs e)
        {
            EncryptionFrame.Navigate(new PlaintextDecryption(gottenId));
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            EncryptionFrame.Navigate(new Help());
        }
    }
}
