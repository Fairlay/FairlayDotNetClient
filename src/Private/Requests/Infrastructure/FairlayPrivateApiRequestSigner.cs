using System.Security.Cryptography;

namespace FairlayDotNetClient.Private.Requests.Infrastructure
{
	public class FairlayPrivateApiRequestSigner : PrivateApiRequestSigner
	{
		public void SetRsaParameters(RSAParameters rsaParameters)
			=> currentRsaParameters = rsaParameters;

		private RSAParameters currentRsaParameters;

		public SignedPrivateApiRequest SignRequest(PrivateApiRequest request, long nonce)
		{
			string signableString = request.FormatIntoSignableString(nonce);
			var signature = SigningExtensions.SignStringUsingSha512(signableString,
				currentRsaParameters);
			return new SignedPrivateApiRequest(request, signature, nonce);
		}
	}
}