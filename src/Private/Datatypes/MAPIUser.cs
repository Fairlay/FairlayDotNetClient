namespace FairlayDotNetClient.Private.Datatypes
{
	public class MAPIUser
	{
		public long ID;
		public bool CanSettleMarkets;
		public bool CanChangeTime;
		public bool CanDoAdminStuff;
		public bool CanDoSpecialRequests;
		public bool PrivForceSignature;
		public bool PrivForceNonce;
		public string PrivPublicKey;
		public decimal DailySpendingLimit;
		public decimal DailySpent;
		public bool ReadOnly;
		public bool TransferOnly;
	}
}