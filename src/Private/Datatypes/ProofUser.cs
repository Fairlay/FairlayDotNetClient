using System;
using System.Collections.Generic;

namespace FairlayDotNetClient.Private.Datatypes
{
	[Serializable]
	public class ProofUser
	{
		public string name;
		public int ID;
		public decimal balance;
		[NonSerialized]
		public List<ProofBlindBranch> branches;

		public ProofUser(string n, decimal b)
		{
			name = n;
			balance = b;
			branches = new List<ProofBlindBranch>();
		}

		public string GetHash()
			=> SigningExtensions.HashSHA256(name + SigningExtensions.DecimalToString(balance));

		public static bool VerifyUserBranches(ProofBlindBranch[] branches, string userName,
			decimal userBalance, string tophash, decimal sumFunds)
		{
			if (branches[branches.Length - 1].hash != tophash)
				return false;
			if (branches[branches.Length - 1].balance > sumFunds)
				return false;
			string userhash = new ProofUser(userName, userBalance).GetHash();
			if (branches[0].hash != userhash)
				return false;
			for (int i = 0; i < branches.Length - 1; i++)
			{
				var pb1 = ProofBranch.MakeBranch(branches[i].hash, branches[i].GetNeighbours()[0].hash,
					branches[i].balance, branches[i].GetNeighbours()[0].balance, new List<ProofUser>());
				var pb2 = ProofBranch.MakeBranch(branches[i].GetNeighbours()[0].hash, branches[i].hash,
					branches[i].GetNeighbours()[0].balance, branches[i].balance, new List<ProofUser>());
				if (pb1.hash != branches[i + 1].hash && pb2.hash != branches[i + 1].hash)
					return false;
				if (branches[i].GetNeighbours()[0].balance < 0)
					return false;
			}
			return true;
		}
	}
}