using System.Security.Cryptography;

namespace FairlayDotNetClient.Private
{
	public static class RsaKeyPairGenerator
	{
		public static RsaKeyPair GenerateNewRsaKeyPair()
		{
			using (var rsa = RSA.Create())
			{
				var privateKeyParameters = rsa.ExportParameters(true);
				var publicKeyParameters = rsa.ExportParameters(false);
				return new RsaKeyPair(privateKeyParameters, publicKeyParameters);
			}
		}
	}
}