namespace FairlayDotNetClient.Private.Datatypes
{
	/// <summary>
	/// https://github.com/Fairlay/FairlayPrivateAPI#get-user-transactions-82
	/// </summary>
	public enum PaymentType
	{
		/// <summary>
		/// Is probably not valid
		/// </summary>
		None = 0,
		/// <summary>
		/// For user-user transfer, appears in statement tab (like register for SocialMediaBot), not
		/// in payments tab. In any case, there is no description support. ttype = 1 for deposits
		/// </summary>
		Default = 1,
		/// <summary>
		/// Is some system stuff (appears in payments)
		/// ttype = 2 for withdrawals
		/// </summary>
		Payments = 2,
		/// <summary>
		/// This is what we use, appears in statements with description, but not in payments tab
		/// ttype = 3 for p2p payments
		/// </summary>
		StatementDescriptionSupport = 3
	}
}