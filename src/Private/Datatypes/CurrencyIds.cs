using System;
using System.Globalization;

namespace FairlayDotNetClient.Private.Datatypes
{
	public static class CurrencyIds
	{
		/// <summary>
		/// Normally mBTC is used internally in Fairlay, however we also support BTC with segwit in the
		/// Wallet Service (depending on the address used for cashout withdrawals or deposits, starting
		/// with 3 means it is a segwit address).
		/// </summary>
		public const int Mbtc = (int)Coin.BTC;
		public const string MbtcShortName = "mBTC";
		public const int Meth = (int)Coin.ETH;
		public const string MethShortName = "mETH";
		public const string EthShortName = "ETH";
		public const int Mltc = (int)Coin.LTC;
		public const string MltcShortName = "mLTC";
		public const int Mdash = (int)Coin.DASH;
		public const string MdashShortName = "mDASH";
		public const int Mbch = (int)Coin.BCH;
		public const string MbchShortName = "mBCH";
		public const string BchShortName = "BCH";
		public const string BchLongName = "BitcoinCash";
		public const string BtcShortName = "BTC";

		public static int FromCoinShortName(string coinShortName)
		{
			if (string.IsNullOrEmpty(coinShortName))
				return Mbtc;
			switch (coinShortName.ToLowerInvariant())
			{
			case "0":
			case "btc":
			case "bitcoin":
			case "mbtc": return Mbtc;
			case "1":
			case "eth":
			case "ethereum":
			case "meth": return Meth;
			case "2":
			case "ltc":
			case "litcoin":
			case "mltc": return Mltc;
			case "3":
			case "dash":
			case "mdash": return Mdash;
			case "4":
			case "bitcoin cash":
			case "bch":
			case "mbch":
			case "bcc":
			case "mbcc": return Mbch;
			}
			throw new NotSupportedException("Unsupported coin: " + coinShortName);
		}

		public static bool TryFromCoinShortName(string coinShortName, out int currencyId)
		{
			currencyId = default(int);
			try
			{
				currencyId = FromCoinShortName(coinShortName);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static string ToCoinShortName(Coin coin) => ToCoinShortName((int)coin);
		public static string ToCoinShortName(int currencyId)
		{
			switch (currencyId)
			{
			case Mbtc: return "BTC";
			case Meth: return "ETH";
			case Mltc: return "LTC";
			case Mdash: return "DASH";
			case Mbch: return "BCH";
			default: return "BTC"; //ncrunch: no coverage
			}
		}

		public static string ToCoinString(this decimal amount, string coinShortName = "mBTC",
			string format = null)
			=> format != null
				? amount.ToString(format, CultureInfo.InvariantCulture) + " " + coinShortName
				: amount.ToString(CultureInfo.InvariantCulture) + " " + coinShortName;

		public static string ToCoinString5Digits(this decimal amount, string coinShortName = "mBTC")
			=> ToCoinString(amount, coinShortName, "#0.0####");
	}
}