using System;
using System.Threading.Tasks;
using FairlayDotNetClient.Public;

namespace FairlayDotNetClient.Tests.Public
{
	public class MockPublicApi : PublicApi
	{
		protected override Task<string> GetHttpResponse(string method, string parameters)
		{
			if (method.StartsWith("Invalid"))
				return Task.FromResult("");
			if (method == Time)
				return Task.FromResult(DateTime.UtcNow.Ticks.ToString());
			if (method == Competitions)
				return Task.FromResult(@"[""USA - Major League Soccer"",""England - FA Cup""]");
			return Task.FromResult(@"[{
		""Comp"": ""Bitcoin Futarchy"",
		""Descr"": ""This is a derivative contract (CFD) priced in mBTC that tracks the price of ETH. In case there are any hard forks weâ€™ll use the sum of all forks to settle the market. In case one or more hard forks occurs, each of them have to represent at least 10% of the total price. If a Hard Fork that has more than 10% of the total hash power is not publicly traded on the resolution date, the price of this particular hard fork will be determined as the first traded price within 30 days after the resolution date. If a certain Hard Fork is not traded on any reputable exchange and is below 10% of the total Hash Rate for more than 30 days, the price of that Hard Fork will be assumed as 0. The settlement will be based on the Poloniex ETH/mBTC price on January 1st 2018. Rules according to https://fairlay.com/faq/#27 and following apply."",
		""Title"": ""Ethereum Future"",
		""CatID"": 40,
		""ClosD"": ""2027-12-13T01:59:00"",
		""SettlD"": ""2018-01-01T04:00:00"",
		""Status"": 0,
		""Ru"": [{
				""Name"": ""empty"",
				""VisDelay"": 6000,
				""RedA"": 0.0,
				""VolMatched"": 2220.915749224
			}
		],
		""_Type"": 2,
		""_Period"": 1,
		""SettlT"": 1,
		""Comm"": 0.02,
		""MinVal"": 10.0,
		""MaxVal"": 200.0,
		""LastCh"": ""2017-03-30T02:38:00.64077Z"",
		""LastSoftCh"": ""2017-08-28T14:28:10.374234"",
		""LogBugs"": """",
		""OrdBStr"": ""{\""Bids\"":[[80.9595,4.0]],\""Asks\"":[],\""S\"":1}"",
		""Pop"": 0.0,
		""Margin"": 10000.0,
		""ID"": 102302287136
	}]");
		}
	}
}