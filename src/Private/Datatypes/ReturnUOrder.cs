namespace FairlayDotNetClient.Private.Datatypes
{
	public class ReturnUOrder
	{
		public UserOrder _UserOrder;
		public UnmatchedOrder _UnmatchedOrder;
		public override string ToString() => _UserOrder + " " +_UnmatchedOrder;
	}
}