namespace FairlayDotNetClient.Private.Datatypes
{
	public class ReturnMOrder
	{
		public UserOrder _UserOrder;
		public MatchedOrder _MatchedOrder;
		public long _UserUMOrderID;
		public override string ToString() => _UserOrder + " " + _MatchedOrder;
	}
}