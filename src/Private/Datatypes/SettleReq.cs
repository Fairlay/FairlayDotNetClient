using System;

namespace FairlayDotNetClient.Private.Datatypes
{
	public class SettleReq
	{
		public long Mid;
		public int Runner;
		public bool Unsettle;
		public int Win;  //The selected Runner either  won (1)  or lost (2)
		public bool Half;  // Needed for example for markets like   Over/Under 2.25   which is Over / Under Markets 2.5  & 2.0     
		public decimal Dec;  // For Decimal Markets
		public decimal ORed;  // Odds Reduction   (for Horse Racings)
		public int[] VoidRunners; // Voiding for Horse Racing Settlements
		public DateTime Executed;
	}
}