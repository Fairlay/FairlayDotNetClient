using System.Threading.Tasks;
using FairlayDotNetClient.Private.Infrastructure;
using FairlayDotNetClient.Private.Requests.Infrastructure;
using FairlayDotNetClient.Private.Responses;
using Newtonsoft.Json;

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
		public override int OurUserId => requestBuilder.OurUserId;

		public override async Task<string> DoApiRequestAndVerify(int requestHeaderId,
			object requestBody = null)
		{
			string body = requestBody == null ? null : requestBody.GetType().IsPrimitive
				? requestBody.ToString() : requestBody is string ? (string)requestBody
					: JsonConvert.SerializeObject(requestBody);
			var apiResponse = await DoApiRequest(requestHeaderId.ToString(), body);
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