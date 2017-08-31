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
			var parsedResponse = PrivateApiResponseExtensions.
				CreateFromApiResponseMessage(TestData.ApiResponse.FormatIntoApiResponseMessage());
			parsedResponse.AssertIsValueEquals(TestData.ApiResponse);
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