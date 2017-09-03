using System;
using System.Threading.Tasks;
using NUnit.Framework;
using FairlayDotNetClient.Public;

namespace FairlayDotNetClient.Tests.Public
{
	public class PublicApiTests
	{
		[SetUp]
		public void CreateMockApiForNCrunchAndFairlayApiOtherwise()
			=> api = NCrunchExtensions.StartedFromNCrunch
				? (PublicApi)new MockPublicApi() : new FairlayPublicApi();

		private PublicApi api;

		[Test]
		public void InvalidCallThrowsError()
			=> Assert.ThrowsAsync<PublicApi.InvalidResponse>(
				async () => await api.GetServerResponse("Invalid2359"));

		[Test]
		public async Task GetServerTime()
			=> Assert.That(await api.GetServerTime(), Is.GreaterThan(DateTime.UtcNow.AddMinutes(-5)));

		[Test]
		public async Task GetMarkets()
		{
			var markets = await api.GetMarkets(MarketX.Category.BITCOIN);
			Assert.That(markets, Is.Not.Empty);
			Assert.That(markets[0].ToString(), Is.Not.Empty);
			Console.WriteLine(markets[0]);
		}

		[Test]
		public async Task GetCompetitions()
		{
			var competitions = await api.GetCompetitions(MarketX.Category.SOCCER);
			Assert.That(competitions, Is.Not.Empty);
			Console.WriteLine(competitions[0]);
		}
	}
}