namespace FairlayDotNetClient.Private.Datatypes
{
	public class UserOrder
	{
		public int BidOrAsk;
		public long MarketID;
		public int RunnerID;
		public long OrderID;
		public string MatchedSubUser;
		public override string ToString() => BidOrAsk + " "+ MarketID + " "+ RunnerID + "-";
	}
}