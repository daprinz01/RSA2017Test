using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Numerics;
using System.Threading;

namespace RSA2017Test
{
    class RSAMain
    {
        //initialize public variables
        public static BigInteger P { get; set; }
        public static BigInteger Q { get; set; }
        public static BigInteger D { get; set; }
        public static BigInteger To { get; set; }
        public static int E { get; set; }
        public static BigInteger N { get; set; }
        public static BigInteger Dp { get; set; }
        public static BigInteger Dq { get; set; }
        public static BigInteger Qinv { get; set; }
        public static BigInteger Ep { get; set; }
        public static BigInteger Eq { get; set; }

        //method to generate primes
        public static BigInteger Random(int Keysize)
        {

            // Buffer storage.
            //store the random number which is half the size of the required keysize so on multiplication we have a keysize with required key length
            byte[] data = new byte[Keysize/(8*2)];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                    // Fill buffer.
                    rng.GetBytes(data);
                
                rng.Dispose();
            }
            //convert the generated random bytes into a set of string by joining them together
            string num = string.Join(null,data);
            //convert the string to a random number in the required bit lenght
            BigInteger RandomNumber = BigInteger.Parse(num);
            return RandomNumber;
        }
        //method to find the lcm of two numbers 
        public static BigInteger FindLCM(BigInteger a, BigInteger b)
        {
            BigInteger num1, num2;
            if (a > b)
            {
                num1 = a; num2 = b;
            }
            else
            {
                num1 = b; num2 = a;
            }

            for (BigInteger i = 1; i <= num2; i++)
            {
                if (BigInteger.Remainder( BigInteger.Multiply(num1 , i) , num2) == 0)
                {
                    return BigInteger.Multiply(num1, i);
                }
            }
            return num2;
        }

        /*//method to check if the random number is prime although this is slower and for small bit values e.g: 16bits and 32bits
        public static bool CheckIfPrime(BigInteger j)
        {
            Int16 k = 0;
            for (BigInteger i = 1; i <= j; i++)
            {
                if (j % i == 0)
                {
                    k++;
                }
                
            }
            if (k == 2) return true;

            else return false;
        }
        */
        //method to generate prime
        public static BigInteger GetPrime(int Keysize)
        {
            BigInteger prime = 1;
             BigInteger RandomNumber = Random(Keysize);
                
                if (RandomNumber.IsProbablyPrime(10))
                {
                  prime = RandomNumber;
                    return prime;

                }
                else
                {
                   return GetPrime(Keysize);
                }


            
            
        }
        //method to compute the value of the modulus
        public static BigInteger GetN(BigInteger p , BigInteger q)
        {
            return p * q;
        }
        //method to calculate the value of totient
        public static BigInteger GetTotient(BigInteger p , BigInteger q)
        {
            
            return BigInteger.Multiply((p - 1), (q - 1));
        }

        //method to assign prime 65537 to the public exponent E
        public static int GetE()
        {
            return 65537;
        }

        

        // try to get D

        public static BigInteger GetD(BigInteger e, BigInteger PHI)
        {
            BigInteger[] u;
            BigInteger[] v;
            BigInteger q, temp1, temp2, temp3;

            u = new BigInteger[] { 0, 0, 0 };
            v = new BigInteger[] { 0, 0, 0 };

            u[0] = 1; u[1] = 0; u[2] = PHI;
            v[0] = 0; v[1] = 1; v[2] = e;

            while (v[2] != 0)
            {
                q = BigInteger.Divide(u[2], v[2]);
                temp1 = u[0] - q * v[0];
                temp2 = u[1] - q * v[1];
                temp3 = u[2] - q * v[2];
                u[0] = v[0];
                u[1] = v[1];
                u[2] = v[2];
                v[0] = temp1;
                v[1] = temp2;
                v[2] = temp3;
            }
            if (u[1] < 0) return (u[1] + PHI);
            else return (u[1]);
        }






        /*
        public static BigInteger Pow(BigInteger value, BigInteger exponent)
        {
            BigInteger originalValue = value;
            while (exponent-- > 1)
                value = BigInteger.Multiply(value, originalValue);
            return value;
        }*/
        public static  MyRSAProvider.MyMyKeys GenerateKey(int Keysize)
        {
            //initialize constructor
            MyRSAProvider.MyMyKeys MyKeys = new MyRSAProvider.MyMyKeys();
            //Get prime P
            P = GetPrime(Keysize);
            //Get prime Q
            Q = GetPrime(Keysize);
            //calculate the modulus
            N = GetN(P, Q);
            //calculate the totient or phi
            To = GetTotient(P, Q);
            // Get the value of the public exponent...(65537)
            E = GetE();
            //compute the value of the private exponent such that  (D*E % To) = 1 
            D = GetD(E, To);
            //calculate other properties of the private and public keys to be used in chinese remender theorem
            Dp = D % (P - 1);
            Dq = D % (Q - 1);
            Ep = E % (P - 1);
            Eq = E % (Q - 1);
            Qinv = GetD(Q, P); 
           //get the keysize into our constructor
            MyKeys.KeySize = Keysize;
            //do checks to see that everything is in other else calculate again
            if (P == 0 || Q == 0 || N == 0 || To == 0 || D == 0 || P == Q || (D*E % To)!= 1 )
            {
                GenerateKey(Keysize);
            }
            //Add the private key values to the constructor
            MyKeys.PrivateKey.Add(new MyRSAProvider.PrivateKey { D = D.ToString(), N = N.ToString(), Dp = Dp.ToString(), Dq = Dq.ToString(), Qinv = Qinv.ToString(), P = P.ToString(), Q = Q.ToString()});
            //Add the public key values to the constructor
            MyKeys.PublicKey.Add(new MyRSAProvider.PublicKey { E = E.ToString(), N = N.ToString(), Ep = Ep.ToString(), Eq = Eq.ToString(), Qinv = Qinv.ToString(), P = P.ToString(), Q = Q.ToString()});

            return MyKeys;
           
        }

        //Method to Encrypt data
        public static byte[] Encrypt(byte[] dataToEncrypt,BigInteger Ep, BigInteger Eq, BigInteger Qinv,BigInteger P, BigInteger Q,int Keysize,BigInteger E, BigInteger n)
        {
           
            BigInteger[] cipherBytes = new BigInteger[dataToEncrypt.Length];
            string[] strData = new string[dataToEncrypt.Length];

            byte[] byteData = new byte[Keysize];
            byte[] returnBytes = new byte[(Keysize/3)*dataToEncrypt.Length];
            
            if (dataToEncrypt.Length>Keysize)
            {
                MessageBox.Show("Bad Message Length");
            }
           
           
            else
            {
                int count = 0;
                
              for (int j = 0; j < dataToEncrypt.Length; j++)
              {
                    //Each of the data is encrypted by calculating C = M^E mod N where C = dataToEncrypt[j], E = E and N = n
                    cipherBytes[j] = BigInteger.ModPow(dataToEncrypt[j], E, n);
                    

                    //This can also be done using the chinese remainder theorem
                    //cipherBytes[j] = (byte)(BigInteger.ModPow((BigInteger)dataToEncrypt[j], (BigInteger)E, (BigInteger)n));
                    // int m1 = (int)(BigInteger.ModPow(dataToEncrypt[j], Ep, P));
                    //int m2 = (int)(BigInteger.ModPow(dataToEncrypt[j], Eq, Q));
                    //int h = (Qinv * (m1 - m2)) % P;
                    //cipherBytes[j] = (byte)(m2 + (h * Q));

                    //Convert each cipherByte to a string
                    strData[j] = cipherBytes[j].ToString();
                    //Get the byte representation of each cipherByte seperated by the $ sign to distinguish each ciherByte during decryption
                    byteData = Encoding.UTF8.GetBytes(strData[j] + "$");
                    //Using try to catch any 
                    try
                    {
                        for (int i = 0; i < byteData.Length; i++)
                        {
                            returnBytes[i + count] = byteData[i];

                        }
                        count += byteData.Length;
                       
                    }
                    catch (Exception e)
                    {

                        MessageBox.Show(e.ToString());
                    }
                    
                    

                    
                }
            }
            
            return returnBytes;

        }
        public static byte[] Decrypt(byte[] dataToDecrypt, BigInteger Dp,BigInteger Dq, BigInteger Qinv, BigInteger P,BigInteger Q, int Keysize,BigInteger D, BigInteger n)
        {
            //Receive the bytes and convert back to a set of strings containing all the cipher texts
            string NewData = Encoding.UTF8.GetString(dataToDecrypt);
            //Separate the set of strrings to the different cipher texts
            string[] numbers = NewData.Split('$');
            //initialize the array to contain our decrypted messages
            byte[] DecryptedBytes = new byte[numbers.Length-1];
            //loop through the cipher texts to convert them to the required messages
            for (int i = 0; i < numbers.Length-1; i++)
                {
                // doing M = C^D modulus N using the BigInteger class
                DecryptedBytes[i] = (byte)BigInteger.ModPow(BigInteger.Parse(numbers[i]), D, n);
                /*  
                 *  //this can also be done using the chinese remainder theorem
           BigInteger m1 = (BigInteger)(BigInteger.Remainder( Pow(BigInteger.Parse(numbers[i]), Dp),P));
            BigInteger m2 = (BigInteger)(BigInteger.Remainder( Pow(dataToDecrypt[i], Dq), Q));
            BigInteger  h = (Qinv * (m1 - m2)) % P;
            DecryptedBytes[i] = (byte)(m2 + (h * Q));*/ 
            }
            return DecryptedBytes;
        }
        
    }



    //class to test if a number is prime

    public static class PrimeExtensions
    {
        // Random generator (thread safe)
        private static ThreadLocal<Random> s_Gen = new ThreadLocal<Random>(
          () => {
              return new Random();
          }
        );

        // Random generator (thread safe)
        private static Random Gen
        {
            get
            {
                return s_Gen.Value;
            }
        }
        //method to check for prime
        public static Boolean IsProbablyPrime(this BigInteger value, int witnesses = 10)
        {
            if (value <= 1)
                return false;

            if (witnesses <= 0)
                witnesses = 10;

            BigInteger d = value - 1;
            int s = 0;

            while (d % 2 == 0)
            {
                d /= 2;
                s += 1;
            }

            Byte[] bytes = new Byte[value.ToByteArray().LongLength];
            BigInteger a;

            for (int i = 0; i < witnesses; i++)
            {
                do
                {
                    Gen.NextBytes(bytes);

                    a = new BigInteger(bytes);
                }
                while (a < 2 || a >= value - 2);

                BigInteger x = BigInteger.ModPow(a, d, value);
                if (x == 1 || x == value - 1)
                    continue;

                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, value);

                    if (x == 1)
                        return false;
                    if (x == value - 1)
                        break;
                }

                if (x != value - 1)
                    return false;
            }

            return true;
        }
    }

}
