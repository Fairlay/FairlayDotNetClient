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
			Assert.That(request.Header, Is.EqualTo(TestData.NumericRequestHeader));
			AssertRequestBodyIsEmptyString(request);
		}

		private static void AssertRequestBodyIsEmptyString(PrivateApiRequest request)
			=> Assert.That(request.Body, Is.Empty);

		[Test]
		public void BuildWithRegisteredAccountIdAndNumericHeader()
		{
			builder.SetApiUser(TestData.UserId, TestData.RegisteredApiAccountId);
			var request = builder.BuildRequest(TestData.NumericRequestHeader);
			Assert.That(request.UserId, Is.EqualTo(TestData.UserId));
			// https://github.com/Fairlay/PrivateApiDocumentation#use-another-api-account
			Assert.That(request.Header, Is.EqualTo("1025"));
			AssertRequestBodyIsEmptyString(request);
		}

		[Test]
		public void BuildWithNativeAccountIdAndNamedHeader()
		{
			builder.SetApiUser(TestData.UserId, TestData.NativeApiAccountId);
			var request = builder.BuildRequest(TestData.NamedRequestHeader);
			Assert.That(request.UserId, Is.EqualTo(TestData.UserId));
			Assert.That(request.Header, Is.EqualTo(TestData.NamedRequestHeader));
			AssertRequestBodyIsEmptyString(request);
		}

		[Test]
		public void BuildWithRegisteredAccountIdAndNamedHeader()
		{
			builder.SetApiUser(TestData.UserId, TestData.RegisteredApiAccountId);
			var request = builder.BuildRequest(TestData.NamedRequestHeader);
			Assert.That(request.UserId, Is.EqualTo(TestData.UserId));
			Assert.That(request.Header, Is.EqualTo(TestData.NamedRequestHeader));
			AssertRequestBodyIsEmptyString(request);
		}

		[Test]
		public void BuildWithRequestBody()
		{
			builder.SetApiUser(TestData.UserId, TestData.RegisteredApiAccountId);
			var request = builder.BuildRequest(TestData.NamedRequestHeader, TestData.RequestBody);
			Assert.That(request.UserId, Is.EqualTo(TestData.UserId));
			Assert.That(request.Header, Is.EqualTo(TestData.NamedRequestHeader));
			Assert.That(request.Body, Is.EqualTo(TestData.RequestBody));
		}
	}
}