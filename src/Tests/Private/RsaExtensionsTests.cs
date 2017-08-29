using System.Security.Cryptography;
using System.Text;
using FairlayDotNetClient.Private;
using NUnit.Framework;

namespace FairlayDotNetClient.Tests.Private
{
	public class RsaExtensionsTests
	{
		[Test]
		public void EncryptAndDecryptDataWithImportedRsaParameters()
		{
			using (var rsa = RSA.Create())
			{
				var rsaParameters = RsaParametersExtensions.CreateFromXmlString(TestData.PrivateRsaXml);
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
				Assert.That(() => RsaParametersExtensions.CreateFromXmlString(InvalidXml),
					Throws.TypeOf<CryptographicException>());
		}
	}
}