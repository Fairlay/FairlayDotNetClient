using System;

namespace FairlayDotNetClient.Private.Datatypes
{
	public class MUserTransfer
	{
		public MUserTransfer(int from, int to, string descr, int ttype, decimal am, int cur)
		{
			ID2 = (long)(DateTime.UtcNow - new DateTime(2015, 1, 1)).TotalMilliseconds;
			FromU = from;
			ToU = to;
			Descr = descr;
			TType = ttype;
			Amount = am;
			Cur = cur;
		}

		public long ID2;
		/// <summary>
		/// Currency ID to send /  0 is mBTC /1 is Ether  /2 is Litecoin  /3 Dash
		/// </summary>
		public int Cur;
		public int FromU;
		public int ToU;
		public string Descr;
		public int TType;
		public decimal Amount;
		public DateTime ExcludedCreationTime => new DateTime(2015, 1, 1).AddMilliseconds(ID2);
	}
}