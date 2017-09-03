namespace FairlayDotNetClient.Private.Requests.Infrastructure
{
	public interface PrivateApiRequestBuilder
	{
		void SetApiUser(int userId, int apiAccountId);
		int OurUserId { get; }
		int OurApiAccountId { get; }
		PrivateApiRequest BuildRequest(string requestHeader, string requestBody = null);
	}
}