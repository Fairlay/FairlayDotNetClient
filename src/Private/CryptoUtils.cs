using System;
using System.Security.Cryptography;
using System.Text;

namespace FairlayDotNetClient.Private
{
	public static class CryptoUtils
	{
		public static string ComputeMessageSignature(string message, string privateKey)
		{
			using (var rsa = RSA.Create())
			{
				var bytes = Encoding.UTF8.GetBytes(message);
				rsa.ImportFromXmlString(privateKey);
				var signedMessage = rsa.SignData(bytes, HashAlgorithmName.SHA512,
					RSASignaturePadding.Pkcs1);
				return Convert.ToBase64String(signedMessage);
			}
		}
	}
}