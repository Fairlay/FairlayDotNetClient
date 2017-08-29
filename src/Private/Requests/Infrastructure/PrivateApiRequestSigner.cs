using System.Security.Cryptography;

namespace FairlayDotNetClient.Private.Requests.Infrastructure
{
	public interface PrivateApiRequestSigner
	{
		void SetRsaParameters(RSAParameters rsaParameters);
		SignedPrivateApiRequest SignRequest(PrivateApiRequest request, long nonce);
	}
}