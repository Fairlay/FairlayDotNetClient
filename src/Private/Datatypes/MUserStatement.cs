using System;

namespace FairlayDotNetClient.Private.Datatypes
{
	public class MUserStatement
	{
		public MUserStatement(decimal am, decimal bank, string descr, int ttype)
		{
			Descr = descr;
			Bank = bank;
			T = ttype;
			Am = am;
		}
		
		public long ID;
		public string Descr;
		public int T;
		public decimal Am;
		public decimal Bank;
		public DateTime ExcludedCreationTime => new DateTime(2016, 1, 1).AddMilliseconds(ID);
	}
}