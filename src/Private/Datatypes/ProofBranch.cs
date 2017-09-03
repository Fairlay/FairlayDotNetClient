using System;
using System.Collections.Generic;

namespace FairlayDotNetClient.Private.Datatypes
{
	[Serializable]
	public class ProofBranch
	{
		public string hash;
		public decimal balance;
		[NonSerialized]
		public List<ProofUser> users;
		public Dictionary<string, ProofUser> usersDic;

		public ProofBranch(string h, decimal b, List<ProofUser> us)
		{
			hash = h;
			balance = b;
			users = us;
			usersDic = new Dictionary<string, ProofUser>();
			foreach (var user in us)
				usersDic[user.name] = user;
		}

		public static ProofBranch MakeBranch(string obj1, string obj2, decimal balance1, decimal balance2, List<ProofUser> us)
			=> new ProofBranch(SigningExtensions.HashSHA1(obj1 +
				SigningExtensions.DecimalToString(balance1) + obj2 +
				SigningExtensions.DecimalToString(balance2)), balance1 + balance2, us);

		public static ProofBranch MakeLeaf(ProofUser u, decimal balance)
			=> new ProofBranch(u.GetHash(), balance, new List<ProofUser> { u });
	}
}