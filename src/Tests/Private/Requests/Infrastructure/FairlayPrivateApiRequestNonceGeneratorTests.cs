using System;
using FairlayDotNetClient.Private.Requests.Infrastructure;
using NUnit.Framework;

namespace FairlayDotNetClient.Tests.Private.Requests.Infrastructure
{
	public class FairlayPrivateApiRequestNonceGeneratorTests
	{
		[Test]
		public void GeneratedNonceIsCurrentUtcTimeInTicks()
		{
			var nonceGenerator = new FairlayPrivateApiRequestNonceGenerator();
			var utcNow = DateTimeOffset.UtcNow;
			long nonce = nonceGenerator.GenerateNonce();
			var utcFromNonce = new DateTimeOffset().AddTicks(nonce);
			Assert.That(utcFromNonce, Is.EqualTo(utcNow).Within(TimeSpan.FromMilliseconds(1)));
		}
	}
}