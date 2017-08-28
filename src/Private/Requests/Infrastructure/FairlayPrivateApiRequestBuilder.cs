namespace FairlayDotNetClient.Private.Requests.Infrastructure
{
	public class FairlayPrivateApiRequestBuilder : PrivateApiRequestBuilder
	{
		public FairlayPrivateApiRequestBuilder(long userId, int apiAccountId)
		{
			this.userId = userId;
			this.apiAccountId = apiAccountId;
		}

		private readonly long userId;
		private readonly int apiAccountId;

		public PrivateApiRequest BuildRequest(string requestHeader, string requestBody = null)
		{
			if (int.TryParse(requestHeader, out int numericRequestHeader) && apiAccountId > 0)
				requestHeader = (numericRequestHeader + 1000 * apiAccountId).ToString();
			return new PrivateApiRequest(userId, requestHeader, requestBody ?? string.Empty);
		}
	}
}