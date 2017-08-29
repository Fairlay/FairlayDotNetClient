using System;
using FairlayDotNetClient.Private.Requests;

namespace FairlayDotNetClient.Private.Infrastructure
{
	public static class SignedPrivateApiRequestExtensions
	{
		/// <summary>
		/// https://github.com/Fairlay/FairlayDotNetClient/wiki/Private-API#fairlay-private-api-documentation-v0
		/// </summary>
		public static string FormatIntoServerMessage(this SignedPrivateApiRequest request)
			=> $"{Convert.ToBase64String(request.Signature)}|{request.Nonce}|{request.UserId}|" +
			$"{request.Header}|{request.Body}{EndOfDataToken}";

		public const string EndOfDataToken = "<\"ENDOFDATA\">";
	}
}