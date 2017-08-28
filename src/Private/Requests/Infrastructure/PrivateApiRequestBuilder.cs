namespace FairlayDotNetClient.Private.Requests.Infrastructure
{
	public interface PrivateApiRequestBuilder
	{
		PrivateApiRequest BuildRequest(string requestHeader, string requestBody = null);
	}
}