using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuanLyThuVien.Encryption
{
    public static class MaHoa
    {
        public static class Base64
        {
            public static string Encode(string plainText)
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                return System.Convert.ToBase64String(plainTextBytes);
            }
        }

        public static class MD5
        {
            public static string Encode(string input)
            {
                StringBuilder hash = new StringBuilder();
                MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
                byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

                for (int i = 0; i < bytes.Length; i++)
                {
                    hash.Append(bytes[i].ToString("x2"));
                }
                return hash.ToString();
            }
        }

        public static class Affine
        {
            public static string Encode(string plainText, int a, int b)
            {
                string cipherText = "";

                // Put Plain Text (all capitals) into Character Array
                char[] chars = plainText.ToUpper().ToCharArray();

                // Compute e(x) = (ax + b)(mod m) for every character in the Plain Text
                foreach (char c in chars)
                {
                    int x = Convert.ToInt32(c - 65);
                    cipherText += Convert.ToChar(((a * x + b) % 26) + 65);
                }

                return cipherText;
            }


            ///
            /// This function takes cipher text and decrypts it using the Affine Cipher
            /// d(x) = aInverse * (e(x)  b)(mod m).
            ///
            public static string Decode(string cipherText, int a, int b)
            {
                string plainText = "";

                // Get Multiplicative Inverse of a
                int aInverse = MultiplicativeInverse(a);

                // Put Cipher Text (all capitals) into Character Array
                char[] chars = cipherText.ToUpper().ToCharArray();

                // Computer d(x) = aInverse * (e(x)  b)(mod m)
                foreach (char c in chars)
                {
                    int x = Convert.ToInt32(c - 65);
                    if (x - b < 0) 
                        x = Convert.ToInt32(x) + 26;
                    plainText += Convert.ToChar(((aInverse * (x - b)) % 26) + 65);
                }

                return plainText;
            }

            ///
            /// This functions returns the multiplicative inverse of integer a mod 26.
            ///
            public static int MultiplicativeInverse(int a)
            {
                for (int x = 1; x < 27; x++)
                {
                    if ((a * x) % 26 == 1)
                        return x;
                }

                throw new Exception("No multiplicative inverse found!");
            }
        }
    }
}
