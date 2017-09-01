using System;
using FairlayDotNetClient.Private.Responses;
using NUnit.Framework;

namespace FairlayDotNetClient.Tests.Private
{
	public static class TestPrivateApiResponseExtensions
	{
		/// <summary>
		/// https://github.com/Fairlay/PrivateApiDocumentation#fairlay-private-api-documentation-v0
		/// </summary>
		public static string FormatIntoApiResponseMessage(this PrivateApiResponse response) =>
			$"{Convert.ToBase64String(response.Signature)}|{response.Nonce}|{response.ServerId}|" +
			response.Body;

		public static void AssertIsValueEquals(this PrivateApiResponse actualResponse,
			PrivateApiResponse expectedResponse)
		{
			Assert.That(actualResponse, Is.Not.Null);
			Assert.That(actualResponse.Signature, Is.EqualTo(expectedResponse.Signature));
			Assert.That(actualResponse.Nonce, Is.EqualTo(expectedResponse.Nonce));
			Assert.That(actualResponse.ServerId, Is.EqualTo(expectedResponse.ServerId));
			Assert.That(actualResponse.Body, Is.EqualTo(expectedResponse.Body));
		}
	}
}