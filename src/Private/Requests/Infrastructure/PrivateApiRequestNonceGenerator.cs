namespace FairlayDotNetClient.Private.Requests.Infrastructure
{
	interface PrivateApiRequestNonceGenerator
	{
		long GenerateNonce();
	}
}