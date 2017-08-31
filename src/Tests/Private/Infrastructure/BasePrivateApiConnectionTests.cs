using System;
using System.Linq;
using System.Threading.Tasks;
using FairlayDotNetClient.Private.Responses;
using NUnit.Framework;

namespace FairlayDotNetClient.Tests.Private.Infrastructure
{
	public class BasePrivateApiConnectionTests
	{
		[SetUp]
		public void Initilize() => apiConnection = new MockPrivateApiConnection();

		private MockPrivateApiConnection apiConnection;

		[Test]
		public async Task DoRequestWithFakeResponse()
		{
			apiConnection.SetFakeResponse(TestData.ApiResponse);
			var response = await apiConnection.DoApiRequest(TestData.SignedApiRequest);
			response.AssertIsValueEquals(TestData.ApiResponse);
		}

		[Test]
		public async Task DoMultipleRequestWithRandomFakeResponse()
		{
			var random = new Random();
			for (int i = 0; i < 100; i++)
			{
				var randomResponse = NewRandomApiResponse(random);
				apiConnection.SetFakeResponse(randomResponse);
				var response = await apiConnection.DoApiRequest(TestData.SignedApiRequest);
				response.AssertIsValueEquals(randomResponse);
			}
		}

		private static PrivateApiResponse NewRandomApiResponse(Random random)
		{
			var signature = NewRandomSignature(random);
			long nonce = NewRandomNonce(random);
			int serverId = NewRandomServerId(random);
			string body = NewRandomBody(random);
			return new PrivateApiResponse(signature, nonce, serverId, body);
		}

		private static byte[] NewRandomSignature(Random random)
		{
			var signature = new byte[128];
			random.NextBytes(signature);
			return signature;
		}

		private static long NewRandomNonce(Random random)
			=> (long)(random.NextDouble() * long.MaxValue);

		private static int NewRandomServerId(Random random) => random.Next();

		private static string NewRandomBody(Random random)
		{
			int bodySize = random.Next(1, 1000);
			const string Input = "abcdefghijklmnopqrstuvwxyz0123456789[](){};:.,";
			return new string(Enumerable.Range(0, bodySize)
				.Select(x => Input[random.Next(0, Input.Length)])
				.ToArray());
		}
	}
}