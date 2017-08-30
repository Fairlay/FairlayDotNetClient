namespace FairlayDotNetClient.Private.Responses
{
	public class PrivateApiResponse
	{
		public PrivateApiResponse(byte[] signature, long nonce, int serverId, string body)
		{
			Signature = signature;
			Nonce = nonce;
			ServerId = serverId;
			Body = body;
		}

		public byte[] Signature { get; }
		public long Nonce { get; }
		public int ServerId { get; }
		public string Body { get; }
	}
}