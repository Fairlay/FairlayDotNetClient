namespace FairlayDotNetClient.Private.Requests
{
	public interface PrivateApiRequestBuilder
	{
		PrivateApiRequest BuildRequest(string requestHeader, string requestBody = null);
	}
}