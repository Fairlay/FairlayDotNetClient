using System;

namespace FairlayDotNetClient.Private.Requests.Infrastructure
{
	public class FairlayPrivateApiRequestNonceGenerator : PrivateApiRequestNonceGenerator
	{
		public long GenerateNonce() => DateTimeOffset.UtcNow.UtcTicks;
	}
}