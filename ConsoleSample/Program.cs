using FairlayDotNetClient.Public;
using System;
using System.Threading.Tasks;

namespace FairlayDotNetClient.ConsoleSample
{
	public class Program
	{
		public static void Main()
		{
			UsePrivateApi().GetAwaiter().GetResult();
			UsePublicApi().GetAwaiter().GetResult();
		}

		private static Task UsePrivateApi() => Task.CompletedTask;
			/*TODO
			var privateApi = new PrivateApi();
			Console.WriteLine("Your private and public key were saved in the config.txt file:\r\n" + tc.getPublicKey());
			bool suc = tc.init(0);
			var balances = tc.getBalances();
			Console.WriteLine("\r\nYour balances are: " + JsonConvert.SerializeObject(balances));
			//Do a proof of reserve
			//retrieve from https://blockchain.info/address/1EVrCZ3ahUucq3jvAqTTPTZe1tySniXuWi
			decimal sumFunds = 1700240m;
			if (yourusernameoremail != null)
				tc.setPublicUserName(yourusernameoremail);
			bool verifiedProof = tc.VerifyProofOfReserves(balances[0].PrivReservedFunds, yourusernameoremail,
				publictophash, sumFunds);
			Console.WriteLine("\r\nIs your balance verified?  " + verifiedProof);
			var nextPresidentOrderbook2016 = tc.getOrderbook(72633292476);
			*/

		private static async Task UsePublicApi()
		{
			var publicApi = new FairlayPublicApi();
			var markets = await publicApi.GetMarkets(Market.Category.BITCOIN);
			Console.WriteLine("First bitcoin market: " + markets[0]);
			var competitions = await publicApi.GetCompetitions(Market.Category.SOCCER);
			Console.WriteLine("First competition: " + competitions[0]);
		}
	}
}