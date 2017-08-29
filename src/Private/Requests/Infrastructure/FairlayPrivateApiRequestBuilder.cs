namespace FairlayDotNetClient.Private.Requests.Infrastructure
{
	public class FairlayPrivateApiRequestBuilder : PrivateApiRequestBuilder
	{
		public void SetApiUser(long userId, int apiAccountId)
		{
			currentUserId = userId;
			currentApiAccountId = apiAccountId;
		}

		private long currentUserId;
		private int currentApiAccountId;

		public PrivateApiRequest BuildRequest(string requestHeader, string requestBody = null)
		{
			if (int.TryParse(requestHeader, out int numericRequestHeader) && currentApiAccountId > 0)
				requestHeader = (numericRequestHeader + 1000 * currentApiAccountId).ToString();
			return new PrivateApiRequest(currentUserId, requestHeader, requestBody ?? string.Empty);
		}
	}
}