using FairlayDotNetClient.Private.Requests.Infrastructure;
using NUnit.Framework;

namespace FairlayDotNetClient.Tests.Private.Requests.Infrastructure
{
	public class PrivateApiRequestExtensionsTests
	{
		[Test]
		public void TestFormatIntoSignableString()
		{
			var request = TestData.ApiRequest;
			string expectedFormat =
				$"{TestData.RequestNonce}|{request.UserId}|{request.Header}|{request.Body}";
			Assert.That(request.FormatIntoSignableString(TestData.RequestNonce),
				Is.EqualTo(expectedFormat));
		}
	}
}