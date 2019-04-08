namespace FairlayDotNetClient.Private.Datatypes
{
	/// <summary>
	/// https://github.com/Fairlay/FairlayPrivateAPI#get-settlements-85
	/// </summary>
	public enum StatementType
	{
		/// <summary>
		/// 0-99 stands for a transfer (same like ttype in transfer object, <see cref="PaymentType"/>)
		/// includes deposits & cashouts
		/// </summary>
		PaymentNone = 0,
		PaymentDefault = 1,
		PaymentWithdrawals = 2,
		PaymentDescriptionSupport = 3,
		Admin = 100,
		MarketSettlementWithoutCommission = 200,
		MarketSettlementWithCommission = 201,
		Unsettlement = 250,
		CommissionBonus = 300
	}
}