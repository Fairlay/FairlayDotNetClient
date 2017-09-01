using System.Threading.Tasks;
using FairlayDotNetClient.Private.Infrastructure;
using FairlayDotNetClient.Private.Requests.Infrastructure;

namespace FairlayDotNetClient.Private
{
	public class FairlayPrivateApi : PrivateApi
	{
		public FairlayPrivateApi(PrivateApiRequestBuilder requestBuilder,
			PrivateApiRequestSigner requestSigner, PrivateApiRequestNonceGenerator requestNonceGenerator,
			PrivateApiConnection apiConnection)
		{
			this.requestBuilder = requestBuilder;
			this.requestSigner = requestSigner;
			this.requestNonceGenerator = requestNonceGenerator;
			this.apiConnection = apiConnection;
		}

		private readonly PrivateApiRequestBuilder requestBuilder;
		private readonly PrivateApiRequestSigner requestSigner;
		private readonly PrivateApiRequestNonceGenerator requestNonceGenerator;
		private readonly PrivateApiConnection apiConnection;

		public async Task<string> DoRequestAndVerify(string requestHeader, string requestBody = null)
		{
			var request = requestBuilder.BuildRequest(requestHeader, requestBody);
			var signedRequest = requestSigner.SignRequest(request, requestNonceGenerator.GenerateNonce());
			var response = await apiConnection.DoApiRequest(signedRequest);
			//TODO: Validate signature with server public key and check for server error message 
			return response.Body;
		}
	}
}