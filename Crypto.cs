using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairlaySampleClient
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    public static class Crypto
    {

        public static SHA256 sha256 = new SHA256CryptoServiceProvider();
        public static SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
        

     
     
        public static String HashSHA256(string objh)
        {
            return ByteArrayToString(sha256.ComputeHash(Encoding.ASCII.GetBytes(objh)));
        }
        public static String HashSHA1(string objh)
        {
            return ByteArrayToString(sha1.ComputeHash(Encoding.ASCII.GetBytes(objh)));
        }
        public static string decimaltoString(decimal d)
        {
           return d.ToString("0.0#############").Replace(",","."); 
        }
      
     

      
      

        public static string ByteArrayToString(byte[] ba)
        {
            var hex = new StringBuilder(ba.Length * 2);
            hex.Clear();
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
