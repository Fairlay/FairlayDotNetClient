namespace FairlayDotNetClient.Private.Requests.Infrastructure
{
	public interface PrivateApiRequestBuilder
	{
		void SetApiUser(long userId, int apiAccountId);
		PrivateApiRequest BuildRequest(string requestHeader, string requestBody = null);
	}
}