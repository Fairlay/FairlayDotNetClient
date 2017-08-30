using System;
using FairlayDotNetClient.Private.Infrastructure;
using NUnit.Framework;

namespace FairlayDotNetClient.Tests.Private.Infrastructure
{
	public class SignedPrivateApiRequestExtensionsTests
	{
		[Test]
		public void TestFormatIntoApiRequestMessage()
		{
			var request = TestData.SignedApiRequest;
			string signature = Convert.ToBase64String(request.Signature);
			string expectedFormat =
				$"{signature}|{request.Nonce}|{request.UserId}|{request.Header}|{request.Body}" +
					SignedPrivateApiRequestExtensions.EndOfDataToken;
			string serverMessage = request.FormatIntoApiRequestMessage();
			Assert.That(serverMessage, Is.EqualTo(expectedFormat));
		}
	}
}