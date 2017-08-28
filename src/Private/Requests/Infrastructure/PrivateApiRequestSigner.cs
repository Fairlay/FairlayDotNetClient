namespace FairlayDotNetClient.Private.Requests.Infrastructure
{
	public interface PrivateApiRequestSigner
	{
		SignedPrivateApiRequest SignRequest(PrivateApiRequest privateApiRequest);
	}
}