using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FairlayDotNetClient.Public
{
	public abstract class PublicApi
	{
		public async Task<DateTime> GetServerTime()
			=> new DateTime(Convert.ToInt64(await GetServerResponse(Time)));
		protected const string Time = "time";

		public Task<List<Market>> GetMarkets(int category, string[] runnerAnd = null,
			int[] typeOr = null, int[] periodOr = null, bool onlyActive = true)
			=> GetMarkets("\"Cat\":" + category +
				(runnerAnd != null ? ",\"RunnerAND\":[\"" + runnerAnd.ToText("\",\"") + "\"]" : "") +
				(typeOr != null ? ",\"TypeOr\":[" + typeOr.ToText() + "]" : "") +
				(periodOr != null ? ",\"PeriodOr\":[" + periodOr.ToText() + "]" : "") +
				", \"OnlyActive\":" + onlyActive.ToString().ToLower());

		public async Task<List<Market>> GetMarkets(string jsonParameters)
		{
			string response = await GetServerResponse(Markets, jsonParameters);
			var markets = JsonConvert.DeserializeObject<List<Market>>(response);
			foreach (var m in markets)
				if (m.ClosD > DateTime.UtcNow.AddHours(-0.5))
					cachedMarkets[m.ID] = m;
			var rememberIdsToDelete = new List<long>();
			foreach (var m in cachedMarkets)
				if (m.Value.ClosD < DateTime.UtcNow.AddHours(-2))
					rememberIdsToDelete.Add(m.Key); //ncrunch: no coverage
			foreach (long mid in rememberIdsToDelete)
				cachedMarkets.Remove(mid); //ncrunch: no coverage
			return cachedMarkets.Values.ToList();
		}

		protected const string Markets = "markets";
		private readonly Dictionary<long, Market> cachedMarkets = new Dictionary<long, Market>();

		public async Task<List<string>> GetCompetitions(int category)
		{
			string response = await GetServerResponse(Competitions, category.ToString());
			return JsonConvert.DeserializeObject<List<string>>(response);
		}

		protected const string Competitions = "comps";

		public async Task<string> GetServerResponse(string method, string parameters = "")
		{
			if (method != Time && lastServerTimeOffsetCalculated < DateTime.UtcNow.AddMinutes(-10))
			{
				serverTimeOffset = DateTime.UtcNow.Ticks - (await GetServerTime()).Ticks;
				lastServerTimeOffsetCalculated = DateTime.UtcNow;
			}
			if (method == Markets)
				parameters = "{" + parameters + ",\"ToID\":10000,\"SoftChangedAfter\":" +
					JsonConvert.SerializeObject(lastCall.AddSeconds(-10).AddTicks(-serverTimeOffset)) + "}";
			string response = await GetHttpResponse(method, parameters);
			if (string.IsNullOrEmpty(response) || response.Contains("XError") ||
				response == "Not supported")
				throw new InvalidResponse(response);
			lastCall = DateTime.UtcNow;
			return response;
		}

		public class InvalidResponse : Exception
		{
			public InvalidResponse(string response) : base(response) {}
		}

		protected abstract Task<string> GetHttpResponse(string method, string parameters);
		private DateTime lastServerTimeOffsetCalculated = DateTime.MinValue;
		private long serverTimeOffset;
		private DateTime lastCall = DateTime.UtcNow;
	}
}