using System;
using System.Threading.Tasks;
using FairlayDotNetClient.Private;
using NUnit.Framework;

namespace FairlayDotNetClient.Tests.Private
{
	[Explicit]
	public class PrivateApiExtensionsTests
	{
		[SetUp]
		public void Initilize()
			=> privateApi = new FairlayPrivateApiBuilder(TestData.Credentials).Build();

		private PrivateApi privateApi;

		[Test]
		public async Task TestGetServerTime()
		{
			var serverTime = await privateApi.GetServerTime();
			Assert.That(serverTime, Is.EqualTo(DateTimeOffset.UtcNow).Within(TimeSpan.FromMinutes(5)));
		}

		[Test]
		public async Task TestGetTransfers()
		{
			await privateApi.GetTransfers(new DateTimeOffset(new DateTime(2017, 06, 13)));
		}

		[Test]
		public async Task TestGetBalances()
		{
			await privateApi.GetBalances();
		}
	}
}