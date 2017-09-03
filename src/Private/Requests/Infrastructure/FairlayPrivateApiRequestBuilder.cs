namespace FairlayDotNetClient.Private.Requests.Infrastructure
{
	public class FairlayPrivateApiRequestBuilder : PrivateApiRequestBuilder
	{
		public void SetApiUser(int userId, int apiAccountId)
		{
			OurUserId = userId;
			OurApiAccountId = apiAccountId;
		}

		public int OurUserId { get; private set; }
		public int OurApiAccountId { get; private set; }

		public PrivateApiRequest BuildRequest(string requestHeader, string requestBody = null)
		{
			if (int.TryParse(requestHeader, out int numericRequestHeader) && OurApiAccountId > 0)
				requestHeader = (numericRequestHeader + 1000 * OurApiAccountId).ToString();
			return new PrivateApiRequest(OurUserId, requestHeader, requestBody ?? string.Empty);
		}
	}
}