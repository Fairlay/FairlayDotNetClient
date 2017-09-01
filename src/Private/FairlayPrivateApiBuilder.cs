using FairlayDotNetClient.Private.Infrastructure;
using FairlayDotNetClient.Private.Requests.Infrastructure;

namespace FairlayDotNetClient.Private
{
	public class FairlayPrivateApiBuilder : PrivateApiBuilder
	{
		public FairlayPrivateApiBuilder(PrivateApiCredentials credentials) =>
			this.credentials = credentials;

		private readonly PrivateApiCredentials credentials;

		public PrivateApiBuilder UseRequestBuilder(PrivateApiRequestBuilder requestBuilder)
		{
			currentRequestBuilder = requestBuilder;
			currentRequestBuilder.SetApiUser(credentials.UserId, credentials.ApiAccountId);
			return this;
		}

		private PrivateApiRequestBuilder currentRequestBuilder;

		public PrivateApiBuilder UseRequestSigner(PrivateApiRequestSigner requestSigner)
		{
			currentRequestSigner = requestSigner;
			currentRequestSigner.SetRsaParameters(credentials.PrivateRsaParameters);
			return this;
		}

		private PrivateApiRequestSigner currentRequestSigner;

		public PrivateApiBuilder UseRequestNonceGenerator(PrivateApiRequestNonceGenerator
			requestNonceGenerator)
		{
			currentRequestNonceGenerator = requestNonceGenerator;
			return this;
		}

		private PrivateApiRequestNonceGenerator currentRequestNonceGenerator;

		public PrivateApiBuilder UseApiConnection(PrivateApiConnection apiConnection)
		{
			currentApiConnection = apiConnection;
			currentApiConnection.SetEndpoint(credentials.ServerEndPoint);
			return this;
		}

		private PrivateApiConnection currentApiConnection;

		public PrivateApi Build()
		{
			UseDefaultImplementationsIfNotProvided();
			return new FairlayPrivateApi(currentRequestBuilder, currentRequestSigner,
				currentRequestNonceGenerator, currentApiConnection);
		}

		private void UseDefaultImplementationsIfNotProvided()
		{
			if (currentRequestBuilder == null)
				UseRequestBuilder(new FairlayPrivateApiRequestBuilder());
			if (currentRequestSigner == null)
				UseRequestSigner(new FairlayPrivateApiRequestSigner());
			if (currentRequestNonceGenerator == null)
				UseRequestNonceGenerator(new FairlayPrivateApiRequestNonceGenerator());
			if (currentApiConnection == null)
				UseApiConnection(new FairlayPrivateApiConnection());
		}
	}
}