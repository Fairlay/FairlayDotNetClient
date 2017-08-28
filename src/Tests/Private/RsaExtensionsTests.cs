using System.Security.Cryptography;
using System.Text;
using FairlayDotNetClient.Private;
using NUnit.Framework;

namespace FairlayDotNetClient.Tests.Private
{
	public class RsaExtensionsTests
	{
		[Test]
		public void EncryptAndDecryptDataWithImportedRsaKey()
		{
			using (var rsa = RSA.Create())
			{
				rsa.ImportFromXmlString(TestData.PrivateRsaXml);
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
			using (var rsa = RSA.Create())
				Assert.That(() => rsa.ImportFromXmlString(InvalidXml),
					Throws.TypeOf<CryptographicException>());
		}
	}
}