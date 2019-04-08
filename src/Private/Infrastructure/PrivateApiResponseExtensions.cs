using System;
using FairlayDotNetClient.Private.Responses;

namespace FairlayDotNetClient.Private.Infrastructure
{
	public static class PrivateApiResponseExtensions
	{
		public static PrivateApiResponse CreateFromApiResponseMessage(string apiResponse)
		{
			string[] responseComponents = apiResponse.Split('|');
			if (responseComponents.Length < 4)
			{
				Console.WriteLine("Invalid response from Fairlay: "+apiResponse);
				// Can also happen if apiResponse is null or "", which sometimes happens from fairlay 1/day
				throw new InvalidNumberOfResponseComponentsMustBeFour(apiResponse); //ncrunch: no coverage
			}
			byte[] signature = Convert.FromBase64String(responseComponents[0]);
			long nonce = long.Parse(responseComponents[1]);
			int serverId = int.Parse(responseComponents[2]);
			string body = responseComponents[3];
			return new PrivateApiResponse(signature, nonce, serverId, body);
		}

		public class InvalidNumberOfResponseComponentsMustBeFour : Exception
		{
			//ncrunch: no coverage start
			public InvalidNumberOfResponseComponentsMustBeFour(string apiResponse) : base(apiResponse) {}
		}
	}
}