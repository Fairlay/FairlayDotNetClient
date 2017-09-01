using System.Threading.Tasks;
using FairlayDotNetClient.Private.Infrastructure;
using FairlayDotNetClient.Private.Requests.Infrastructure;
using FairlayDotNetClient.Private.Responses;

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

		public async Task<string> DoApiRequestAndVerify(string requestHeader, string requestBody = null)
		{
			var apiResponse = await DoApiRequest(requestHeader, requestBody);
			ThrowIfServerErrorMessage(apiResponse.Body);
			//TODO: Validate signature with server public key
			return apiResponse.Body;
		}

		private Task<PrivateApiResponse> DoApiRequest(string requestHeader, string requestBody)
		{
			var request = requestBuilder.BuildRequest(requestHeader, requestBody);
			var signedRequest = requestSigner.SignRequest(request, requestNonceGenerator.GenerateNonce());
			return apiConnection.DoApiRequest(signedRequest);
		}

		private static void ThrowIfServerErrorMessage(string apiResponseBody)
		{
			string normalizedResponse = apiResponseBody.ToLower();
			if (normalizedResponse.StartsWith("xerror") || normalizedResponse.StartsWith("yerror"))
				throw new FairlayPrivateApiException(apiResponseBody);
		}
	}
}