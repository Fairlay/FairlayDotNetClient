using FairlayDotNetClient.Private.Infrastructure;
using FairlayDotNetClient.Private.Requests.Infrastructure;

namespace FairlayDotNetClient.Private
{
	public interface PrivateApiBuilder
	{
		PrivateApiBuilder UseRequestBuilder(PrivateApiRequestBuilder requestBuilder);
		PrivateApiBuilder UseRequestSigner(PrivateApiRequestSigner requestSigner);
		PrivateApiBuilder UseRequestNonceGenerator(PrivateApiRequestNonceGenerator
			requestNonceGenerator);
		PrivateApiBuilder UseApiConnection(PrivateApiConnection apiConnection);
		PrivateApi Build();
	}
}