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
	}
}