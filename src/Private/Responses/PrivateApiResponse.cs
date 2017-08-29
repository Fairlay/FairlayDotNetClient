namespace FairlayDotNetClient.Private.Responses
{
	public class PrivateApiResponse
	{
		public byte[] Signature { get; }
		public long Nonce { get; }
		public int ServerId { get; }
		public string Body { get; }
	}
}