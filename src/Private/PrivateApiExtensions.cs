using System;
using System.Threading.Tasks;

namespace FairlayDotNetClient.Private
{
	public static class PrivateApiExtensions
	{
		public static async Task<DateTimeOffset> GetServerTime(this PrivateApi privateApi)
		{
			string response = await privateApi.DoApiRequestAndVerify("2");
			long serverTimeTicks = long.Parse(response);
			return new DateTimeOffset().AddTicks(serverTimeTicks);
		}

		public static async Task GetTransfers(this PrivateApi privateApi, DateTimeOffset sinceDate)
		{
			string response = await privateApi.DoApiRequestAndVerify("82", sinceDate.UtcTicks.ToString());
		}

		public static async Task GetBalances(this PrivateApi privateApi, int currencyID = -1)
		{
			string response = await privateApi.DoApiRequestAndVerify("122", currencyID.ToString());
		}
	}
}