namespace FairlayDotNetClient.Private.Requests
{
	public class SignedPrivateApiRequest : PrivateApiRequest
	{
		public SignedPrivateApiRequest(PrivateApiRequest apiRequest, byte[] signature, long nonce)
			: base(apiRequest.UserId, apiRequest.RequestHeader, apiRequest.RequestBody)
		{
			Signature = signature;
			Nonce = nonce;
		}

		public byte[] Signature { get; }
		public long Nonce { get; }
	}
}