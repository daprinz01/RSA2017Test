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
    /// Interaction logic for Help.xaml
    /// </summary>
    public partial class Help : Page
    {
        public Help()
        {
            InitializeComponent();

            string helpurl1 = ((Directory.GetCurrentDirectory() + "\\ReadMe.txt"));
            //System.Diagnostics.Process.Start(helpurl1);
            About.Content = System.IO.File.ReadAllText(helpurl1);

       /*     "READ ME\n\n" +


"This project was done by\n"+
"Odele Charles O.\n"+
"REG NO.	2012 / 181445\n\n"+
"and\n\n"+
"Okechukwu Prince O.\n\n"+

"Supervisor:\n"+
           "Dr.U.A.Nnolim\n\n\n"+

            "About The Project:\n"+
"This project implements the RSA Encryption and Decryption Scheme.\n"+
"The purpose of the project is to increase data security and security of information as information\n"+
is transfered from one point to another.

The security of RSA is base on the mathematical difficulty of factorizing out the two prime numbers form
their product and on the difficulty of getting the private decryption key from the public encryption key as 
no known method has been discovered to do this.

How to Navigate through the software:
1.	Run the.exe of software.
2.	If user doesn't already have an account, click on signup.
3.	Enter the required details and select the key size you wish to use.
4.	Click on Sign up to register user and generate key for the user.
5.	To encrypt, enter text or load the text you want to encrypt.
6.	Select the user for which you want to encrypt the message for to load the user's public key.
7.	Click on the encrypt button to encrypt the data.
8.	Click on the save butt to save the encrypted file for sending or decryption.
9.	To decrypt a file, load the file to the decrypted file textbox by clicking on the button in the sele file groupbox.
10. For simulation purposes, select the user for whom the file was encrypted for.
11.	Click on the decrypt button to decrypt the file.
12. Save the decrypted file by clicking the save button.
13. Continue using the software or close after operation has been completed.



For enquiries and recommendations:
okechukwuprince @hotmail.com

or

charlesodele2 @gmail.com.

Thanks for using our software.


Enjoy data security in this information age!!!

";*/
        }
    }
}
