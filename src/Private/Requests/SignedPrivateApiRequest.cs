namespace FairlayDotNetClient.Private.Requests
{
	public class SignedPrivateApiRequest : PrivateApiRequest
	{
		public SignedPrivateApiRequest(PrivateApiRequest request, byte[] signature, long nonce)
			: base(request.UserId, request.Header, request.Body)
		{
			Signature = signature;
			Nonce = nonce;
		}

		public byte[] Signature { get; }
		public long Nonce { get; }
	}
}