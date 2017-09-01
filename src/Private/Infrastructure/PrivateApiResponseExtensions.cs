using System;
using FairlayDotNetClient.Private.Responses;

namespace FairlayDotNetClient.Private.Infrastructure
{
	public static class PrivateApiResponseExtensions
	{
		public static PrivateApiResponse CreateFromApiResponseMessage(string apiResponse)
		{
			var responseComponents = apiResponse.Split('|');
			var signature = Convert.FromBase64String(responseComponents[0]);
			long nonce = long.Parse(responseComponents[1]);
			int serverId = int.Parse(responseComponents[2]);
			string body = responseComponents[3];
			return new PrivateApiResponse(signature, nonce, serverId, body);
		}
	}
}