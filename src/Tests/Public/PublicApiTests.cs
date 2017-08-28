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
		public async Task GetServerTime()
			=> Assert.That(await api.GetServerTime(), Is.GreaterThan(DateTime.UtcNow.AddMinutes(-5)));
	}
}