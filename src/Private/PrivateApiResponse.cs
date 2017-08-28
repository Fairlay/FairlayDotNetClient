namespace FairlayDotNetClient.PrivateApi
{
	public class PrivateApiResponse
	{
		public string Signature { get; set; }
		public long Nonce { get; set; }
		public int ServerId { get; set; }
		public string Payload { get; set; }
		public PrivateApiResponseStatus Status { get; set; }
	}
}