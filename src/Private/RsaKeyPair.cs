using System.Security.Cryptography;

namespace FairlayDotNetClient.Private
{
	public class RsaKeyPair
	{
		public RsaKeyPair(RSAParameters privateKeyParameters, RSAParameters publicKeyParameters)
		{
			PrivateKeyParameters = privateKeyParameters;
			PublicKeyParameters = publicKeyParameters;
		}

		public RSAParameters PrivateKeyParameters { get; }
		public RSAParameters PublicKeyParameters { get; }
	}
}