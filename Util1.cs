using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FairlaySampleClient
{
    public static class Util1
    {
        public static DateTime getUTCNow
        {
            get
            {
                DateTime t2 = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);

                var sou = TimeZoneInfo.FindSystemTimeZoneById("UTC");


                try
                {
                    return TimeZoneInfo.ConvertTime(t2, TimeZoneInfo.Local, sou);
                }
                catch (Exception ex)
                {
                    return TimeZoneInfo.ConvertTime(t2.AddMinutes(60), TimeZoneInfo.Local, sou);

                }

            }

        }

  
        public static byte[] Zip(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    CopyTo(msi, gs);
                }

                return mso.ToArray();
            }
        }
        public static string RemoveDiacritics(string text)
        {
            if (text == null) return null;
            text = text.Replace("ı", "i").Replace("\"", "").Replace("ø", "o").Replace("đ", "d").Replace("?", "").Replace("Å", "A").Replace("ə", "e").Replace("\"", "").Replace("\\", "").Replace("ł", "l").Replace("Ł", "L").Replace("Đ", "D").Replace("Ð", "D").Replace("ộ", "o");

            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {



                    stringBuilder.Append(c);
                }
            }

            var ret = stringBuilder.ToString().Normalize(NormalizationForm.FormC);

            stringBuilder = new StringBuilder();

            foreach (var c in ret)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                int cv = Convert.ToInt32(c);
                if (cv < 130)
                {
                    stringBuilder.Append(c);
                }
            }


            return stringBuilder.ToString();
        }
  
        public static string Unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    //gs.CopyTo(mso);
                    CopyTo(gs, mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }

        public static string SignData(string message, string privateKey)
        {
            byte[] signedBytes;
            using (var rsa = new RSACryptoServiceProvider())
            {
                //// Write the message to a byte array using UTF8 as the encoding.
                var encoder = new UTF8Encoding();
                byte[] originalData = encoder.GetBytes(message);

                try
                {
                    //// Import the private key used for signing the message
                    rsa.FromXmlString(privateKey);

                    //// Sign the data, using SHA512 as the hashing algorithm 
                    signedBytes = rsa.SignData(originalData, CryptoConfig.MapNameToOID("SHA512"));
                }
                catch (CryptographicException e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
                finally
                {
                    //// Set the keycontainer to be cleared when rsa is garbage collected.
                    rsa.PersistKeyInCsp = false;
                }
            }
            //// Convert the a base64 string before returning
            return Convert.ToBase64String(signedBytes);
        }

        public static bool VerifyData(string originalMessage, string signedMessage, string publicKey)
        {
            bool success = false;
            using (var rsa = new RSACryptoServiceProvider())
            {

                var encoder = new UTF8Encoding();
                byte[] bytesToVerify = encoder.GetBytes(originalMessage);

                byte[] signedBytes = Convert.FromBase64String(signedMessage);
                try
                {
                    rsa.FromXmlString(publicKey);

                    SHA512Managed Hash = new SHA512Managed();

                    byte[] hashedData = Hash.ComputeHash(signedBytes);

                    success = rsa.VerifyData(bytesToVerify, CryptoConfig.MapNameToOID("SHA512"), signedBytes);
                }
                catch (CryptographicException e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
            return success;
        }


        public static string generatePrivKey()
        {

            //Encrypt and export public and private keys
            var rsa1 = new RSACryptoServiceProvider();
            string publicPrivateXml = rsa1.ToXmlString(true);   // <<<<<<< HERE

            string publicKey = rsa1.ToXmlString(false);


            return publicPrivateXml;

        }

        public static string getPubKey(string priv)
        {

            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(priv);

            return rsa.ToXmlString(false);
        }
          private static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }
    }
}
