namespace FairlayDotNetClient.Private.Requests
{
	public class PrivateApiRequest
	{
		public PrivateApiRequest(long userId, string header, string body)
		{
			UserId = userId;
			Header = header;
			Body = body;
		}

		public long UserId { get; }
		public string Header { get; }
		public string Body { get; }
	}
}