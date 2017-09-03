using System;
using System.Collections.Generic;

namespace FairlayDotNetClient.Private.Datatypes
{
	[Serializable]
	public class ProofBlindBranch
	{
		public string hash;
		public decimal balance;
		public int layer;
		public List<ProofBlindBranch> neighbours;

		public ProofBlindBranch(string h, decimal b, int l)
		{
			hash = h;
			balance = b;
			layer = l;
			neighbours = new List<ProofBlindBranch>();
		}

		public void AddNeighbour(ProofBlindBranch n) => neighbours.Add(n);
		public ProofBlindBranch[] GetNeighbours() => neighbours.ToArray();

		public override bool Equals(object obj)
		{
			var other = obj as ProofBlindBranch;
			if (other == null)
				return false;
			return hash.Equals(other.hash) && balance.Equals(other.balance) && layer == other.layer;
		}

		public override int GetHashCode()
		{
			int hashCode = 2124593419;
			// ReSharper disable NonReadonlyMemberInGetHashCode
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(hash);
			hashCode = hashCode * -1521134295 + balance.GetHashCode();
			hashCode = hashCode * -1521134295 + layer.GetHashCode();
			hashCode = hashCode * -1521134295 + EqualityComparer<List<ProofBlindBranch>>.Default.GetHashCode(neighbours);
			return hashCode;
		}
	}
}