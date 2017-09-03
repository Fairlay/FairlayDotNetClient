namespace FairlayDotNetClient.Private.Datatypes
{
	public class Currency
	{
		public int ID;
		public string Name;
		public string Symbol;
		public string ColdWalletAddress;
		public decimal TotalBalance;
		/// <summary>
		/// User Account which maintaines the Currency
		/// </summary>
		public long Maintainer;
	}
}