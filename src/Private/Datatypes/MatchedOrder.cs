using System;
using System.Globalization;
using FairlayDotNetClient.Public;

namespace FairlayDotNetClient.Private.Datatypes
{
	public class MatchedOrder : IComparable<MatchedOrder>
	{
		/// <summary>
		/// When two open orders are matched, a Matched Order is created in the PENDING state. If the
		/// maker of the bet cancels his bet within a certain time period (usually 0, 3 or 6 seconds
		/// depending on the market) the bet goes into the state MAKERVOIDED and is void. When a market
		/// is settled the orders go in one of the settled states VOID, WON, HALFWON, LOST or HALFLOST.
		/// Decimal market go into the state DECIMALRESULT while settlement value DecResult will be set.
		/// </summary>
		public enum MOState
		{
			MATCHED,
			RUNNERWON,
			RUNNERHALFWON,
			RUNNERLOST,
			RUNNERHALFLOST,
			MAKERVOIDED,
			VOIDED,
			PENDING,
			DECIMALRESULT,
			DECIMALRESULTTOBASE
		}

		public decimal DecResult;
		public int R;
		public long ID;
		public MOState State;
		public decimal Price;
		public decimal Amount;
		public int MakerCancelTime;
		public DateTime ExcludedCreationTime => new DateTime(ID);

		public static decimal getOrderLiability(int bidorask, decimal minmaxAmount, decimal price,
			decimal amount, MarketX.SettleType settlT)
		{
			if (settlT == MarketX.SettleType.BINARY)
				return bidorask == 1 ? amount : amount * (price - 1);
			if (settlT == MarketX.SettleType.DECIMAL)
				return bidorask == 0 ? (price - minmaxAmount) * amount : (minmaxAmount - price) * amount;
			if (settlT == MarketX.SettleType.DECIMALTOBASE)
				return bidorask == 0 ? (price / minmaxAmount - 1) * amount
					: (1 - price / minmaxAmount) * amount;
			return decimal.MaxValue;
		}

		public int CompareTo(MatchedOrder other) => ID.CompareTo(other.ID);

		public override string ToString()
			=> Price.ToString(CultureInfo.InvariantCulture) + " " +
				Amount.ToString(CultureInfo.InvariantCulture) + " "+ ExcludedCreationTime + " " + State;
	}
}