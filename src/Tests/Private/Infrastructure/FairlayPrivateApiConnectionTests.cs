using System;
using System.Threading.Tasks;
using FairlayDotNetClient.Private.Infrastructure;
using FairlayDotNetClient.Private.Requests.Infrastructure;
using NUnit.Framework;

namespace FairlayDotNetClient.Tests.Private.Infrastructure
{
	public class FairlayPrivateApiConnectionTests
	{
		[SetUp]
		public void Initilize()
		{
			requestBuilder = new FairlayPrivateApiRequestBuilder();
			requestBuilder.SetApiUser(TestData.UserId, TestData.RegisteredApiAccountId);
			nonceGenerator = new FairlayPrivateApiRequestNonceGenerator();
			requestSigner = new FairlayPrivateApiRequestSigner();
			requestSigner.SetRsaParameters(TestData.ClientPrivateRsaParameters);
			apiConnection = new FairlayPrivateApiConnection();
		}

		private PrivateApiRequestBuilder requestBuilder;
		private PrivateApiRequestNonceGenerator nonceGenerator;
		private PrivateApiRequestSigner requestSigner;
		private FairlayPrivateApiConnection apiConnection;

		[Test, Explicit]
		public async Task DoGetServerTimeRequestToRealApiServer()
		{
			apiConnection.SetEndpoint(TestData.ApiServerEndPoint);
			const string GetServerTimeRequestHeader = "2";
			var request = requestBuilder.BuildRequest(GetServerTimeRequestHeader);
			var signedRequest = requestSigner.SignRequest(request, nonceGenerator.GenerateNonce());
			var response = await apiConnection.DoApiRequest(signedRequest);
			Assert.That(response, Is.Not.Null);
			Assert.That(response.Body, Is.Not.Empty);
			long serverTimeTicks = long.Parse(response.Body);
			var serverTime = new DateTimeOffset().AddTicks(serverTimeTicks);
			Assert.That(serverTime, Is.EqualTo(DateTimeOffset.UtcNow).Within(TimeSpan.FromMinutes(5)));
		}
	}
}