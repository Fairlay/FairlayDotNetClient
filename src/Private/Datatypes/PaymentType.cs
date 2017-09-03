namespace FairlayDotNetClient.Private.Datatypes
{
	public enum PaymentType
	{
		/// <summary>
		/// Is probably not valid
		/// </summary>
		None = 0,
		/// <summary>
		/// For user-user transfer, appears in statement tab (like register for SocialMediaBot), not
		/// in payments tab (but maybe sometimes too). In any case, there is no description support.
		/// </summary>
		Default = 1,
		/// <summary>
		/// Is some system stuff (appears in payments)
		/// </summary>
		Payments = 2,
		/// <summary>
		/// This is what we use, appears in statements with description, but not in payments tab
		/// </summary>
		StatementDescriptionSupport = 3
	}
}