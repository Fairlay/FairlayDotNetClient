using FairlayDotNetClient.Private.Requests;
using NUnit.Framework;

namespace FairlayDotNetClient.Tests.Private.Requests
{
	public class FairlayPrivateApiRequestBuilderTests
	{
		[Test]
		public void BuildWithNativeAccountIdAndNumericHeader()
		{
			var builder = new FairlayPrivateApiRequestBuilder(TestData.UserId,
				TestData.NativeApiAccountId);
			var request = builder.BuildRequest(TestData.NumericRequestHeader);
			Assert.That(request.UserId, Is.EqualTo(TestData.UserId));
			Assert.That(request.RequestHeader, Is.EqualTo(TestData.NumericRequestHeader));
			AssertRequestBodyIsEmptyString(request);
		}

		private static void AssertRequestBodyIsEmptyString(PrivateApiRequest request)
			=> Assert.That(request.RequestBody, Is.Empty);

		[Test]
		public void BuildWithRegisteredAccountIdAndNumericHeader()
		{
			var builder = new FairlayPrivateApiRequestBuilder(TestData.UserId,
				TestData.RegisteredApiAccountId);
			var request = builder.BuildRequest(TestData.NumericRequestHeader);
			Assert.That(request.UserId, Is.EqualTo(TestData.UserId));
			// https://github.com/Fairlay/PrivateApiDocumentation#use-another-api-account
			Assert.That(request.RequestHeader, Is.EqualTo("2025"));
			AssertRequestBodyIsEmptyString(request);
		}

		[Test]
		public void BuildWithNativeAccountIdAndNamedHeader()
		{
			var builder = new FairlayPrivateApiRequestBuilder(TestData.UserId,
				TestData.NativeApiAccountId);
			var request = builder.BuildRequest(TestData.NamedRequestHeader);
			Assert.That(request.UserId, Is.EqualTo(TestData.UserId));
			Assert.That(request.RequestHeader, Is.EqualTo(TestData.NamedRequestHeader));
			AssertRequestBodyIsEmptyString(request);
		}

		[Test]
		public void BuildWithRegisteredAccountIdAndNamedHeader()
		{
			var builder = new FairlayPrivateApiRequestBuilder(TestData.UserId,
				TestData.RegisteredApiAccountId);
			var request = builder.BuildRequest(TestData.NamedRequestHeader);
			Assert.That(request.UserId, Is.EqualTo(TestData.UserId));
			Assert.That(request.RequestHeader, Is.EqualTo(TestData.NamedRequestHeader));
			AssertRequestBodyIsEmptyString(request);
		}

		[Test]
		public void BuildWithRequestBody()
		{
			var builder = new FairlayPrivateApiRequestBuilder(TestData.UserId,
				TestData.RegisteredApiAccountId);
			var request = builder.BuildRequest(TestData.NumericRequestHeader, TestData.RequestBody);
			Assert.That(request.UserId, Is.EqualTo(TestData.UserId));
			Assert.That(request.RequestBody, Is.EqualTo(TestData.RequestBody));
		}
	}
}