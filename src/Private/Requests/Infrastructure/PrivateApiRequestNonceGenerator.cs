namespace FairlayDotNetClient.Private.Requests.Infrastructure
{
	public interface PrivateApiRequestNonceGenerator
	{
		long GenerateNonce();
	}
}