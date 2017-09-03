namespace FairlayDotNetClient.Private.Datatypes
{
	public class REQChangeOrder
	{
		public long Mid;
		//RunnerID  0:1st runner; 1: 2nd runner; ....
		public int Rid;
		// Order ID  (should be set to -1 if no old order shall be replaced)
		public long Oid;
		//Amount in mBTC
		// In case of ask orders this amount represents the liability, in case of bid orders this amount represents the possible winnings. 
		public decimal Am;
		// Set the price to 0  if you like to cancel an order and not create a new one.
		public decimal Pri;
		//Text for your own reference
		public string Sub;
		public UnmatchedOrder.Type Type;
		// Must be 0 for Bid Orders  and 1 for Ask.  Ask means that you bet on the outcome to happen.
		public int Boa;
		//This is the Maker cancel time.  How much time you have as maker of the bet to void the bet after the intial matching. 
		// should be set to 0 by default. Is only interesting for professional market makers. Learn more about it by reading how our matching process works in the README.rst
		public int Mct;
		//Set this to true if you place a bid order on a binary market and you like to have the Amount as Liability of the Order.
		public bool LayAsL;
		//Order will be cancelled  at this time in Ticks in UTC, set above 0 to use  if  it's below the current time in ticks, the order won't be created.
		public long CAt;
		//Response   returns either an json encoded UnmatchedOrder object or an XError
		public string Res;
	}
}