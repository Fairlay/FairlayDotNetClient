using System;

namespace FairlayDotNetClient.Private.Datatypes
{
	public class MUserTransfer : IEquatable<MUserTransfer>
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
		/// Currency ID to send:
		/// 0 is mBTC (with or without Segwit Support)
		/// 1 is mETH,
		/// 2 is mLTC, 
		/// 3 is mDASH,
		/// 4 is mBCH
		/// </summary>
		public int Cur;
		public int FromU;
		public int ToU;
		public string Descr;
		/// <summary>
		/// <see cref="PaymentType"/>
		/// </summary>
		public int TType;
		public decimal Amount;
		public DateTime ExcludedCreationTime => new DateTime(2015, 1, 1).AddMilliseconds(ID2);

		public bool Equals(MUserTransfer other)
		{
			if (ReferenceEquals(null, other))
				return false;
			if (ReferenceEquals(this, other))
				return true;
			return ID2 == other.ID2 && FromU == other.FromU && ToU == other.ToU && Cur == other.Cur;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;
			if (ReferenceEquals(this, obj))
				return true;
			return obj.GetType() == GetType() && Equals((MUserTransfer)obj);
		}

		// ReSharper disable NonReadonlyMemberInGetHashCode
		public override int GetHashCode() => ID2.GetHashCode();
	}
}