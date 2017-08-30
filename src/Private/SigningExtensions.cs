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
	}
}