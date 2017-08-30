using System;
using FairlayDotNetClient.Private.Responses;

namespace FairlayDotNetClient.Private.Infrastructure
{
	public static class PrivateApiResponseExtensions
	{
		public static PrivateApiResponse CreateFromApiResponseMessage(string apiResponse)
		{
			ThrowIfServerErrorMessage(apiResponse);
			return ParseApiResponse(apiResponse);
		}

		private static void ThrowIfServerErrorMessage(string apiResponse)
		{
			string normalizedResponse = apiResponse.ToLower();
			if (normalizedResponse.StartsWith("xerror") || normalizedResponse.StartsWith("yerror"))
				throw new FairlayPrivateApiException(apiResponse);
		}

		private static PrivateApiResponse ParseApiResponse(string apiResponse)
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