using System.Security.Cryptography;
using System.Text;
using FairlayDotNetClient.Private.Requests;
using FairlayDotNetClient.Private.Requests.Infrastructure;
using NUnit.Framework;

namespace FairlayDotNetClient.Tests.Private.Requests.Infrastructure
{
	public class FairlayPrivateApiRequestSignerTests
	{
		[SetUp]
		public void Initilize()
		{
			signer = new FairlayPrivateApiRequestSigner();
			signer.SetRsaParameters(TestData.ClientPrivateRsaParameters);
		}

		private FairlayPrivateApiRequestSigner signer;

		[Test]
		public void TestSignRequest()
		{
			var request = TestData.ApiRequest;
			var signedRequest = signer.SignRequest(request, TestData.RequestNonce);
			Assert.That(signedRequest.UserId, Is.EqualTo(request.UserId));
			Assert.That(signedRequest.Header, Is.EqualTo(request.Header));
			Assert.That(signedRequest.Body, Is.EqualTo(request.Body));
			Assert.That(signedRequest.Nonce, Is.EqualTo(TestData.RequestNonce));
			AssertSignatureIsValide(request, signedRequest);
		}

		private static void AssertSignatureIsValide(PrivateApiRequest request,
			SignedPrivateApiRequest signedRequest)
		{
			using (var rsa = RSA.Create())
			{
				rsa.ImportParameters(TestData.ClientPrivateRsaParameters);
				string signableString = request.FormatIntoSignableString(TestData.RequestNonce);
				var signableStringData = Encoding.UTF8.GetBytes(signableString);
				bool isValidSignature = rsa.VerifyData(signableStringData, signedRequest.Signature,
					HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
				Assert.That(isValidSignature, Is.True);
			}
		}
	}
}