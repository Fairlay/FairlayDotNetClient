namespace FairlayDotNetClient.Private.Datatypes
{
	public class ReturnBalance
	{
		public decimal PrivReservedFunds;
		public decimal MaxFunds;
		public decimal PrivUsedFunds;
		public decimal AvailableFunds;
		public decimal SettleUsed;
		public decimal CreatorUsed;
		public int RemainingRequests;
	}
}