using System.Security.Cryptography;
using System.Text;

namespace FairlayDotNetClient.Private
{
	public static class SigningExtensions
	{
		public static byte[] SignStringUsingSha512(string content, RSAParameters rsaParameters)
		{
			using (var rsa = RSA.Create())
			{
				var contentData = Encoding.UTF8.GetBytes(content);
				rsa.ImportParameters(rsaParameters);
				return rsa.SignData(contentData, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
			}
		}

		public static string HashSHA256(string data)
			=> ByteArrayToString(sha256.ComputeHash(Encoding.ASCII.GetBytes(data)));
		public static SHA256 sha256 = new SHA256CryptoServiceProvider();

		public static string HashSHA1(string data)
			=> ByteArrayToString(sha1.ComputeHash(Encoding.ASCII.GetBytes(data)));
		public static SHA1 sha1 = new SHA1CryptoServiceProvider();

		public static string ByteArrayToString(byte[] ba)
		{
			var hex = new StringBuilder(ba.Length * 2);
			hex.Clear();
			foreach (byte b in ba)
				hex.AppendFormat("{0:x2}", b);
			return hex.ToString();
		}

		public static string DecimalToString(decimal d)
			=> d.ToString("0.0#############").Replace(",", ".");
	}
}