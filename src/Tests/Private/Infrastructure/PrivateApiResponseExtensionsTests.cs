using System;
using FairlayDotNetClient.Private;
using FairlayDotNetClient.Private.Infrastructure;
using NUnit.Framework;

namespace FairlayDotNetClient.Tests.Private.Infrastructure
{
	public class PrivateApiResponseExtensionsTests
	{
		[Test]
		public void CreateFromValidApiResponseMessage()
		{
			var parsedResponse =
				PrivateApiResponseExtensions.CreateFromApiResponseMessage(TestData.ApiResponseMessage);
			var actualResponse = TestData.ApiResponse;
			Assert.That(parsedResponse, Is.Not.Null);
			Assert.That(parsedResponse.Signature, Is.EqualTo(actualResponse.Signature));
			Assert.That(parsedResponse.Nonce, Is.EqualTo(actualResponse.Nonce));
			Assert.That(parsedResponse.ServerId, Is.EqualTo(actualResponse.ServerId));
			Assert.That(parsedResponse.Body, Is.EqualTo(actualResponse.Body));
		}

		[TestCase("XError: general error")]
		[TestCase("YError: error in a subtask of a bulk change order reqeust")]
		public void CreateFromServerErrorMessageThrowsException(string apiReponse)
		{
			var exception = Assert.Throws<FairlayPrivateApiException>(() =>
				PrivateApiResponseExtensions.CreateFromApiResponseMessage(apiReponse));
			Assert.That(exception.Message, Is.EqualTo(apiReponse));
		}
	}
}