using System;
using System.Globalization;

namespace FairlayDotNetClient.Private.Datatypes
{
	public static class CurrencyIds
	{
		public const int Mbtc = 0;
		public const string MbtcShortName = "mBTC";
		public const int Meth = 1;
		public const string MethShortName = "mETH";
		public const string EthShortName = "ETH";
		public const int Mltc = 2;
		public const string MltcShortName = "mLTC";
		public const int Mdash = 3;
		public const string MdashShortName = "mDASH";
		public const int Mbch = 4;
		public const string MbchShortName = "mBCH";
		public const string BchShortName = "BCH";
		public const string BchLongName = "BitcoinCash";

		public static int FromCoinShortName(string coinShortName)
		{
			switch (coinShortName.ToLowerInvariant())
			{
				case "btc":
				case "mbtc": return Mbtc;
				case "eth":
				case "meth": return Meth;
				case "ltc":
				case "mltc": return Mltc;
				case "dash":
				case "mdash": return Mdash;
				case "bch":
				case "mbch":
				case "bcc":
				case "mbcc": return Mbch;
			}
			throw new NotSupportedException("Unsupported coin: " + coinShortName);
		}

		public static string ToCoinString(this decimal amount, string coinShortName = "mBTC")
			=> amount.ToString(CultureInfo.InvariantCulture) + " " + coinShortName;
	}
}