using System;
using System.Security.Cryptography;
using System.Text;
using NUnit.Framework;

namespace FairlayDotNetClient.Tests
{
	public class CryptoUtilsTests
	{
		[Test]
		public void TestComputeMessageSignature()
		{
			const string Message = "Hello World";
			string messageSignature = CryptoUtils.ComputeMessageSignature(Message, TestData.PrivateRsaXml);
			using (var rsa = RSA.Create())
			{
				rsa.ImportFromXmlString(TestData.PrivateRsaXml);
				var messageData = Encoding.UTF8.GetBytes(Message);
				var messageSignatureData = Convert.FromBase64String(messageSignature);
				bool isValidSignature = rsa.VerifyData(messageData, messageSignatureData,
					HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
				Assert.That(isValidSignature, Is.True);
			}
		}
	}
}