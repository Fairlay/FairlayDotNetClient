using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using FairlayDotNetClient.Private;
using NUnit.Framework;

namespace FairlayDotNetClient.Tests.Private
{
	public class RsaParametersExtensionsTests
	{
		[Test]
		public void EncryptAndDecryptDataWithImportedRsaParameters()
		{
			using (var rsa = RSA.Create())
			{
				var rsaParameters = RsaParametersExtensions.FromXmlString(TestData.ClientPrivateRsaXml);
				rsa.ImportParameters(rsaParameters);
				var originalData = Encoding.UTF8.GetBytes("Hello World");
				var encryptedData = rsa.Encrypt(originalData, RSAEncryptionPadding.Pkcs1);
				var decryptedData = rsa.Decrypt(encryptedData, RSAEncryptionPadding.Pkcs1);
				Assert.That(decryptedData, Is.EqualTo(originalData));
			}
		}

		[Test]
		public void XmlWithoutRSAKeyValueThrowsException()
		{
			const string InvalidXml = "<Banana>Hello World</Banana>";
				Assert.That(() => RsaParametersExtensions.FromXmlString(InvalidXml),
					Throws.TypeOf<CryptographicException>());
		}

		[Test]
		public void PrivateRsaParametersToString()
		{
			var expectedRsaParameters =
				RsaParametersExtensions.FromXmlString(TestData.ClientPrivateRsaXml);
			string xmlString = expectedRsaParameters.ToXmlString();
			var rsaParameters = RsaParametersExtensions.FromXmlString(xmlString);
			AssertRsaParametersAreEqual(rsaParameters, expectedRsaParameters);
		}

		[Test]
		public void PublicRsaParametersToString()
		{
			var expectedRsaParameters =
				RsaParametersExtensions.FromXmlString(TestData.ClientPublicRsaXml);
			string xmlString = expectedRsaParameters.ToXmlString();
			var rsaParameters = RsaParametersExtensions.FromXmlString(xmlString);
			AssertRsaParametersAreEqual(rsaParameters, expectedRsaParameters);
		}

		private static void AssertRsaParametersAreEqual(RSAParameters rsaParameters,
			RSAParameters expectedRsaParameters)
		{
			Assert.That(rsaParameters.D, Is.EqualTo(expectedRsaParameters.D));
			Assert.That(rsaParameters.DP, Is.EqualTo(expectedRsaParameters.DP));
			Assert.That(rsaParameters.DQ, Is.EqualTo(expectedRsaParameters.DQ));
			Assert.That(rsaParameters.Exponent, Is.EqualTo(expectedRsaParameters.Exponent));
			Assert.That(rsaParameters.InverseQ, Is.EqualTo(expectedRsaParameters.InverseQ));
			Assert.That(rsaParameters.Modulus, Is.EqualTo(expectedRsaParameters.Modulus));
			Assert.That(rsaParameters.P, Is.EqualTo(expectedRsaParameters.P));
			Assert.That(rsaParameters.Q, Is.EqualTo(expectedRsaParameters.Q));
		}
	}
}