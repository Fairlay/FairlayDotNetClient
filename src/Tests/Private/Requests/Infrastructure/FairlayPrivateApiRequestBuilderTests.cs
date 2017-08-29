using FairlayDotNetClient.Private.Requests;
using FairlayDotNetClient.Private.Requests.Infrastructure;
using NUnit.Framework;

namespace FairlayDotNetClient.Tests.Private.Requests.Infrastructure
{
	public class FairlayPrivateApiRequestBuilderTests
	{
		[SetUp]
		public void Initilize() => builder = new FairlayPrivateApiRequestBuilder();

		private FairlayPrivateApiRequestBuilder builder;

		[Test]
		public void BuildWithNativeAccountIdAndNumericHeader()
		{
			builder.SetApiUser(TestData.UserId, TestData.NativeApiAccountId);
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
			builder.SetApiUser(TestData.UserId, TestData.RegisteredApiAccountId);
			var request = builder.BuildRequest(TestData.NumericRequestHeader);
			Assert.That(request.UserId, Is.EqualTo(TestData.UserId));
			// https://github.com/Fairlay/PrivateApiDocumentation#use-another-api-account
			Assert.That(request.RequestHeader, Is.EqualTo("2025"));
			AssertRequestBodyIsEmptyString(request);
		}

		[Test]
		public void BuildWithNativeAccountIdAndNamedHeader()
		{
			builder.SetApiUser(TestData.UserId, TestData.NativeApiAccountId);
			var request = builder.BuildRequest(TestData.NamedRequestHeader);
			Assert.That(request.UserId, Is.EqualTo(TestData.UserId));
			Assert.That(request.RequestHeader, Is.EqualTo(TestData.NamedRequestHeader));
			AssertRequestBodyIsEmptyString(request);
		}

		[Test]
		public void BuildWithRegisteredAccountIdAndNamedHeader()
		{
			builder.SetApiUser(TestData.UserId, TestData.RegisteredApiAccountId);
			var request = builder.BuildRequest(TestData.NamedRequestHeader);
			Assert.That(request.UserId, Is.EqualTo(TestData.UserId));
			Assert.That(request.RequestHeader, Is.EqualTo(TestData.NamedRequestHeader));
			AssertRequestBodyIsEmptyString(request);
		}

		[Test]
		public void BuildWithRequestBody()
		{
			builder.SetApiUser(TestData.UserId, TestData.RegisteredApiAccountId);
			var request = builder.BuildRequest(TestData.NumericRequestHeader, TestData.RequestBody);
			Assert.That(request.UserId, Is.EqualTo(TestData.UserId));
			Assert.That(request.RequestBody, Is.EqualTo(TestData.RequestBody));
		}
	}
}