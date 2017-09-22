using FairlayDotNetClient.Public;
using System;
using System.Net;
using System.Threading.Tasks;
using FairlayDotNetClient.Private;
using FairlayDotNetClient.Private.Datatypes;
using Newtonsoft.Json;

namespace FairlayDotNetClient.ConsoleSample
{
	public class Program
	{
		public static void Main()
		{
			UsePrivateApi().GetAwaiter().GetResult();
			UsePublicApi().GetAwaiter().GetResult();
		}

		private static async Task UsePrivateApi()
		{
			var privateApi = new FairlayPrivateApiBuilder(new PrivateApiCredentials
			{
				UserId = 1004056,
				ApiAccountId = 1,
				PrivateRsaParameters = RsaParametersExtensions.FromXmlString(ClientPrivateRsaXml),
				ServerEndPoint = new IPEndPoint(IPAddress.Parse("31.172.83.53"), 18017)
			}).Build();
			var balances = await privateApi.GetBalances();
			Console.WriteLine("Your current available balance: " +
				balances[CurrencyIds.Mbtc].AvailableFunds.ToCoinString());
			Console.WriteLine("Your balances details: " + JsonConvert.SerializeObject(balances));
			var statements = await privateApi.GetStatement(DateTime.UtcNow.AddMonths(-1));
			Console.WriteLine("Your statements of the past month are: " +
				JsonConvert.SerializeObject(statements));
		}

		public const string ClientPrivateRsaXml =
			"<RSAKeyValue><Modulus>5ZuQEg2qtTrYvehQTxdBJiOSyNiBpBW+fVlk7HJ9+6OQmGCNhnjLfcGFMA1ODxsiC8ILlI5++l2H8SCtDLNMGi2O5aDBsY9Bs+QetQAJVKyFtsoDRtBvy4vlR/joMP+jic/Mt2r92riFGWunZliPI47qiPVJd8JycXR2Kf8VbCU=</Modulus><Exponent>AQAB</Exponent><P>/b6BwxydWyLh1RAaJtO4ROiRNYUB14TAzjfleylapPfT64HlDyLbMzolg0zbH1FDq6D17mEB+Ac3m9+cKHWpzQ==</P><Q>56Yfum0pgh4Yi0WKzmiu/rKYVkVqjz0acjczKtgaUjLEsE2vUM4B9N+8S2EXci5TZ/dCsYktMuD157Ea5sGTuQ==</Q><DP>sR4zka/9nsoAfSraNlP/AgqcoZEQMhH2S3v08T1yikh7Yp6u9xvLijyCRt507a1Z4Qlf4V9RcoIHLQSvCgbn2Q==</DP><DQ>01Bv0RiTrObXbPTbUr+cIyu4W7qnIlOTNG22d3b7S9CULGAxdXz3u/H9SqYfRUGNRGICrQF+AdPFfr3I1IfxwQ==</DQ><InverseQ>dA/bkeMYF3YO1+2ZQftJ5M1tcyBocrYu30q3ztNwqegWwKmBNQc+GxM/OY0ybTQBkuwX3IMo80KUgHj7puOgfw==</InverseQ><D>oAyvAZabPxcVDFPK30bTd/VmFTCuNaWekhvlONiaLvWWDlGHdYvwNOnXoGY12lvMgKuzjMtDgdv+rbtcRTUaPvbW14ZBLQ0FakuNtOwCax8uGGSeFsmBoLXJXTWJdtrlCyV1N581X+7yfeKjE+xXT1KLT0ojUCOqEq/yEQtZMeE=</D></RSAKeyValue>";

		private static async Task UsePublicApi()
		{
			var publicApi = new FairlayPublicApi();
			var markets = await publicApi.GetMarkets(MarketX.Category.BITCOIN);
			Console.WriteLine("First bitcoin market: " + markets[0]);
			var competitions = await publicApi.GetCompetitions(MarketX.Category.SOCCER);
			Console.WriteLine("First competition: " + competitions[0]);
		}
	}
}