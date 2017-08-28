using System;
using System.Threading.Tasks;
using FairlayDotNetClient.Public;

namespace FairlayDotNetClient.Tests.Public
{
	public class MockPublicApi : PublicApi
	{
		protected override Task<string> GetHttpResponse(string method, string jsonParameters,
			DateTime softChangedAfter)
		{
			if (method == Time)
				return Task.FromResult(DateTime.UtcNow.Ticks.ToString());
			return Task.FromResult("TODO");
		}
	}
}