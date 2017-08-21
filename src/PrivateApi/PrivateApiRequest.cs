namespace FairlayDotNetClient.PrivateApi
{
	public class PrivateApiRequest
	{
		public string Signature { get; set; }
		public long Nonce { get; set; }
		public long UserId { get; set; }
		public int RequestId { get; set; }
		public string Payload { get; set; }
	}
}