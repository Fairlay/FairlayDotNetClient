namespace FairlayDotNetClient.Private.Requests
{
	public class PrivateApiRequest
	{
		public PrivateApiRequest(long userId, string requestHeader, string requestBody)
		{
			UserId = userId;
			RequestHeader = requestHeader;
			RequestBody = requestBody;
		}

		//public string Signature { get; set; }
		//public long Nonce { get; set; }
		public long UserId { get; }
		public string RequestHeader { get; }
		public string RequestBody { get; }
	}
}