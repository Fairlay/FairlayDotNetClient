using System;
using System.Threading.Tasks;
using FairlayDotNetClient.Private;
using NUnit.Framework;

namespace FairlayDotNetClient.Tests.Private
{
	[Explicit]
	public class PrivateApiTests
	{
		[SetUp]
		public void Initilize()
			=> privateApi = new FairlayPrivateApiBuilder(TestData.Credentials).Build();


		private PrivateApi privateApi;

		[Test]
		public async Task TestGetServerTime()
		{
			var serverTime = await privateApi.GetServerTime();
			Assert.That(serverTime, Is.EqualTo(DateTimeOffset.UtcNow).Within(TimeSpan.FromMinutes(5)));
		}

		[Test]
		public async Task TestGetTransfers()
		{
			var transfers = await privateApi.GetTransfers(new DateTimeOffset(new DateTime(2017, 06, 13)));
			foreach (var transfer in transfers)
				Console.WriteLine("Transfer from " + transfer.FromU + " to " + transfer.ToU + ": " +
					transfer.Amount + " " + GetCurrencyName(transfer.Cur) + ", Description: " +
					transfer.Descr);
		}

		private static string GetCurrencyName(int currencyId)
			=> currencyId == 0 ? "mBTC" : currencyId == 1 ? "mETH" : currencyId == 2 ? "mLTC" : "mDASH";

		[Test]
		public async Task TestGetBalances()
		{
			var balances = await privateApi.GetBalances();
			foreach (var balance in balances)
				Console.WriteLine(
					"Balance: " + balance.Value.AvailableFunds + " " + GetCurrencyName(balance.Key));
		}

		[Test]
		public async Task TestGetApiAccounts()
		{
			var accounts = await privateApi.GetApiAccounts();
			foreach (var account in accounts)
				Console.WriteLine("Account ID: " + account.ID + " DailySpent: " + account.DailySpent +
					" ReadOnly: " + account.ReadOnly);
		}
	}
}