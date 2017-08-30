using System.Security.Cryptography;
using System.Text;
using FairlayDotNetClient.Private;
using NUnit.Framework;

namespace FairlayDotNetClient.Tests.Private
{
	public class SigningExtensionsTests
	{
		[Test]
		public void SignStringWithSha512AndVerfiy()
		{
			const string Content = "Hello World";
			var contentSignatureData = SigningExtensions.SignStringUsingSha512(Content,
				TestData.ClientPrivateRsaParameters);
			using (var rsa = RSA.Create())
			{
				rsa.ImportParameters(TestData.ClientPrivateRsaParameters);
				var contentData = Encoding.UTF8.GetBytes(Content);
				bool isValidSignature = rsa.VerifyData(contentData, contentSignatureData,
					HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
				Assert.That(isValidSignature, Is.True);
			}
		}
	}
}