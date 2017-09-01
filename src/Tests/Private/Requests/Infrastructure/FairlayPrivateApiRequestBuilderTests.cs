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
		public void BuildWithNativeApiAccountIdAndNumericHeader()
		{
			builder.SetApiUser(TestData.Credentials.UserId, TestData.NativeApiAccountId);
			var request = builder.BuildRequest(TestData.ApiRequest.Header);
			Assert.That(request.UserId, Is.EqualTo(TestData.Credentials.UserId));
			Assert.That(request.Header, Is.EqualTo(TestData.ApiRequest.Header));
			AssertIsEmptyRequestBody(request);
		}

		private static void AssertIsEmptyRequestBody(PrivateApiRequest request)
			=> Assert.That(request.Body, Is.Empty);

		[Test]
		public void BuildWithRegisteredApiAccountIdAndNumericHeader()
		{
			builder.SetApiUser(TestData.Credentials.UserId, TestData.Credentials.ApiAccountId);
			var request = builder.BuildRequest(TestData.ApiRequest.Header);
			Assert.That(request.UserId, Is.EqualTo(TestData.Credentials.UserId));
			// Numeric header with non-native api account id has to be header + 1000 * api_account_id
			// https://github.com/Fairlay/PrivateApiDocumentation#use-another-api-account
			Assert.That(request.Header, Is.EqualTo("1025"));
			AssertIsEmptyRequestBody(request);
		}

		[Test]
		public void BuildWithNativeApiAccountIdAndNamedHeader()
		{
			builder.SetApiUser(TestData.Credentials.UserId, TestData.NativeApiAccountId);
			var request = builder.BuildRequest(TestData.NamedRequestHeader);
			Assert.That(request.UserId, Is.EqualTo(TestData.Credentials.UserId));
			Assert.That(request.Header, Is.EqualTo(TestData.NamedRequestHeader));
			AssertIsEmptyRequestBody(request);
		}

		[Test]
		public void BuildWithRegisteredApiAccountIdAndNamedHeader()
		{
			builder.SetApiUser(TestData.Credentials.UserId, TestData.Credentials.ApiAccountId);
			var request = builder.BuildRequest(TestData.NamedRequestHeader);
			Assert.That(request.UserId, Is.EqualTo(TestData.Credentials.UserId));
			Assert.That(request.Header, Is.EqualTo(TestData.NamedRequestHeader));
			AssertIsEmptyRequestBody(request);
		}

		[Test]
		public void BuildWithRequestBody()
		{
			builder.SetApiUser(TestData.Credentials.UserId, TestData.Credentials.ApiAccountId);
			var request = builder.BuildRequest(TestData.NamedRequestHeader, TestData.ApiRequest.Body);
			Assert.That(request.UserId, Is.EqualTo(TestData.Credentials.UserId));
			Assert.That(request.Header, Is.EqualTo(TestData.NamedRequestHeader));
			Assert.That(request.Body, Is.EqualTo(TestData.ApiRequest.Body));
		}
	}
}