using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FairlayDotNetClient.Public
{
	public abstract class PublicApi
	{
		public async Task<DateTime> GetServerTime()
			=> new DateTime(Convert.ToInt64(await GetServerResponse(Time)));
		protected const string Time = "time";

		public Task<List<Market>> GetMarkets() => null;

		private async Task<string> GetServerResponse(string method, string jsonParameters = "")
		{
			if (method != Time && lastServerTimeOffsetCalculated < DateTime.UtcNow.AddMinutes(-10))
			{
				serverTimeOffset = DateTime.UtcNow.Ticks - (await GetServerTime()).Ticks;
				lastServerTimeOffsetCalculated = DateTime.UtcNow;
			}
			var softChangedAfter = lastCall.AddSeconds(-10).AddTicks(-serverTimeOffset);
			string response = await GetHttpResponse(method, jsonParameters, softChangedAfter);
			if (string.IsNullOrEmpty(response) || response.Contains("XError"))
				throw new InvalidResponse(response);
			lastCall = DateTime.UtcNow;
			return response;
		}

		private class InvalidResponse : Exception
		{
			public InvalidResponse(string response) : base(response) {}
		}

		protected abstract Task<string> GetHttpResponse(string method, string jsonParameters,
			DateTime softChangedAfter);
		private DateTime lastServerTimeOffsetCalculated = DateTime.MinValue;
		private long serverTimeOffset;
		private DateTime lastCall = DateTime.UtcNow;
	}
}