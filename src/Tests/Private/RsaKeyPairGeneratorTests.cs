using System;
using System.Security.Cryptography;
using FairlayDotNetClient.Private;
using NUnit.Framework;

namespace FairlayDotNetClient.Tests.Private
{
	public class RsaKeyPairGeneratorTests
	{
		[Test]
		public void TestGenerateNewRsaKeyPair()
		{
			var rsaKeyPair = RsaKeyPairGenerator.GenerateNewRsaKeyPair();
			AssertPrivateKeyParameters(rsaKeyPair.PrivateKeyParameters);
			AssertPublicKeyParameters(rsaKeyPair.PublicKeyParameters);
		}

		private static void AssertPrivateKeyParameters(RSAParameters privateKeyParameters)
		{
			Assert.That(privateKeyParameters.D.Length, Is.EqualTo(256));
			Assert.That(privateKeyParameters.DP.Length, Is.EqualTo(128));
			Assert.That(privateKeyParameters.DQ.Length, Is.EqualTo(128));
			Assert.That(privateKeyParameters.Exponent.Length, Is.EqualTo(3));
			Assert.That(privateKeyParameters.InverseQ.Length, Is.EqualTo(128));
			Assert.That(privateKeyParameters.Modulus.Length, Is.EqualTo(256));
			Assert.That(privateKeyParameters.P.Length, Is.EqualTo(128));
			Assert.That(privateKeyParameters.Q.Length, Is.EqualTo(128));
		}

		private static void AssertPublicKeyParameters(RSAParameters privateKeyParameters)
		{
			Assert.That(privateKeyParameters.D, Is.Null);
			Assert.That(privateKeyParameters.DP, Is.Null);
			Assert.That(privateKeyParameters.DQ, Is.Null);
			Assert.That(privateKeyParameters.Exponent.Length, Is.EqualTo(3));
			Assert.That(privateKeyParameters.InverseQ, Is.Null);
			Assert.That(privateKeyParameters.Modulus.Length, Is.EqualTo(256));
			Assert.That(privateKeyParameters.P, Is.Null);
			Assert.That(privateKeyParameters.Q, Is.Null);
		}

		/// <summary>
		/// https://github.com/Fairlay/FairlayDotNetClient#generate-rsa-key-pair-using-net
		/// </summary>
		[Test, Category("Slow")]
		public void CreateRsaKeyPair()
		{
			var rsaKeyPair = RsaKeyPairGenerator.GenerateNewRsaKeyPair();
			Console.WriteLine("Private Key: " + rsaKeyPair.PrivateKeyParameters.ToXmlString());
			Console.WriteLine("Public Key: " + rsaKeyPair.PublicKeyParameters.ToXmlString());
		}
	}
}