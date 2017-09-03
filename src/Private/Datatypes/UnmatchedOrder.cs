using System;
using System.Globalization;

namespace FairlayDotNetClient.Private.Datatypes {
	public class UnmatchedOrder : IComparable<UnmatchedOrder>
	{
		public enum UOState
		{
			Active,
			Cancelled,
			Matched,
			MatchedAndCancelled
		}

		public enum Type
		{
			MAKERTAKER,
			MAKER,
			TAKER
		}
		public decimal Price { get; set; }
		public decimal RemAmount { get; set; }
		public Type _Type;
		public decimal ExcludedWinnings => (Price - 1) * RemAmount;
		public UOState State { get; set; }
		public int BidOrAsk { get; set; }
		public decimal PrivAmount { get; set; }
		public long PrivID;
		public string PrivSubUser { get; set; }
		public int makerCT { get; set; }
		public long PrivUserID;
		public DateTime ExcludedCreationTime => new DateTime(PrivID);
		public UnmatchedOrder(bool layliability, long uid, int bidorask, decimal price, decimal amount,
			Type typ, string subUser, int mCTime)
		{
			PrivUserID = uid;
			BidOrAsk = bidorask;
			Price = price;
			PrivAmount = amount;
			if (layliability) PrivAmount = Math.Round(amount / (price - 1), 5);
			RemAmount = amount;
			PrivSubUser = subUser;
			makerCT = mCTime;
			_Type = typ;
			State = UOState.Active;
		}

		public int CompareTo(UnmatchedOrder other)
		{
			if (other == null) return -1;
			if (Price != other.Price)
				return BidOrAsk == 1 ? Price.CompareTo(other.Price) : other.Price.CompareTo(Price);
			return PrivID.CompareTo(other.PrivID);
		}

		public override string ToString()
			=> BidOrAsk + " "+Price.ToString(CultureInfo.InvariantCulture) + " "+
				RemAmount.ToString(CultureInfo.InvariantCulture) + " "+PrivSubUser;
	}
}