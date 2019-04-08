using System.Net;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FairlayDotNetClient.Private;
using FairlayDotNetClient.Private.Infrastructure;
using FairlayDotNetClient.Private.Requests;
using FairlayDotNetClient.Private.Requests.Infrastructure;
using FairlayDotNetClient.Private.Responses;
using NUnit.Framework;

namespace FairlayDotNetClient.Tests.Private
{
	public class FairlayPrivateApiBuilderTests
	{
		[SetUp]
		public void Initialize() => builder = new FairlayPrivateApiBuilder(TestData.Credentials);
		private FairlayPrivateApiBuilder builder;

		[Test]
		public void TestUseRequestBuilder()
		{
			var requestBuilder = new DummyRequestBuilderSpy();
			Assert.That(builder.UseRequestBuilder(requestBuilder), Is.EqualTo(builder));
			Assert.That(requestBuilder.OurUserId, Is.EqualTo(TestData.Credentials.UserId));
			Assert.That(requestBuilder.OurApiAccountId, Is.EqualTo(TestData.Credentials.ApiAccountId));
			AssertCanFairlayPrivateApi();
		}

		private void AssertCanFairlayPrivateApi()
		{
			PrivateApi privateApi = null;
			Assert.That(() => privateApi = builder.Build(), Throws.Nothing);
			Assert.That(privateApi, Is.Not.Null);
		}

		[Test]
		public void TestUseRequestSigner()
		{
			var requestSigner = new DummyRequestSignerSpy();
			Assert.That(builder.UseRequestSigner(requestSigner), Is.EqualTo(builder));
			Assert.That(requestSigner.RsaParameters,
				Is.EqualTo(TestData.Credentials.PrivateRsaParameters));
			AssertCanFairlayPrivateApi();
		}

		[Test]
		public void TestUseRequestNonceGenerator()
		{
			Assert.That(builder.UseRequestNonceGenerator(new DummyRequestNonceGenerator()),
				Is.EqualTo(builder));
			AssertCanFairlayPrivateApi();
		}

		[Test]
		public void TestUseApiConnection()
		{
			var apiConnection = new DummyApiConnectionSpy();
			Assert.That(builder.UseApiConnection(apiConnection), Is.EqualTo(builder));
			Assert.That(apiConnection.EndPoint, Is.EqualTo(TestData.Credentials.ServerEndPoints[0]));
			AssertCanFairlayPrivateApi();
		}

		private class DummyRequestBuilderSpy : PrivateApiRequestBuilder
		{
			public void SetApiUser(int userId, int apiAccountId)
			{
				OurUserId = userId;
				OurApiAccountId = apiAccountId;
			}

			public int OurUserId { get; private set; }
			public int OurApiAccountId { get; private set; }
			public PrivateApiRequest BuildRequest(string requestHeader, string requestBody = null)
				=> null; //ncrunch: no coverage
		}

		private class DummyRequestSignerSpy : PrivateApiRequestSigner
		{
			public void SetRsaParameters(RSAParameters rsaParameters) => RsaParameters = rsaParameters;
			public RSAParameters RsaParameters { get; private set; }
			public SignedPrivateApiRequest SignRequest(PrivateApiRequest request, long nonce)
				=> null; //ncrunch: no coverage
		}

		private class DummyRequestNonceGenerator : PrivateApiRequestNonceGenerator
		{
			public long GenerateNonce() => 0; //ncrunch: no coverage
		}

		private class DummyApiConnectionSpy : PrivateApiConnection
		{
			public void SetEndPoints(List<IPEndPoint> endPoints) => EndPoint = endPoints[0];
			public IPEndPoint EndPoint { get; private set; }
			public Task<PrivateApiResponse> DoApiRequest(SignedPrivateApiRequest request)
				=> null; //ncrunch: no coverage
		}
	}
}