using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace FairlaySampleClient
{
    [Serializable]
    public class ProofUser
    {
        public String name;
        public int ID;
        public decimal balance;
        [NonSerialized]
        public List<ProofBlindBranch> branches;

        public ProofUser(String n, decimal b)
        {
            name = n;
            balance = b;
            branches = new List<ProofBlindBranch>();
        }

        public String GetHash()
        {
          //  var test = Convert.ToString(balance);
    //         var test = String.Format("{0:d}", balance);
          
            var xf = Crypto.HashSHA256(name + Crypto.decimaltoString(balance));
             return xf;
        }

        public static bool VerifyUserBranches(ProofBlindBranch[] branches, String userName, decimal userBalance, String tophash)
        {
            if (branches[branches.Length - 1].hash != tophash) return false;
            //			Console.WriteLine("TOP HASH VALIDATED");
            ProofUser user = new ProofUser(userName, userBalance);
            var userhash = user.GetHash();
            var pb = ProofBranch.MakeLeaf(user, user.balance);
          //  var bb = new ProofBlindBranch(l.hash, l.balance, 1);
			
           // user.ID = 1004056;
          //  user.balance = 1m;
            if (branches[0].hash != pb.hash) return false;
            //			Console.WriteLine("USER HASH VALIDATED");
            ProofBranch pb1, pb2;
            for (int i = 0; i < branches.Length - 1; i++)
            {
                pb1 = ProofBranch.MakeBranch(branches[i].hash, branches[i].GetNeighbours()[0].hash, branches[i].balance, branches[i].GetNeighbours()[0].balance, new List<ProofUser>());
                pb2 = ProofBranch.MakeBranch(branches[i].GetNeighbours()[0].hash, branches[i].hash, branches[i].GetNeighbours()[0].balance, branches[i].balance, new List<ProofUser>());
                if (pb1.hash != branches[i + 1].hash && pb2.hash != branches[i + 1].hash) return false;
                if (branches[i].GetNeighbours()[0].balance < 0) return false;
                //				Console.WriteLine("LAYER "+i+" BRANCH HASH VALIDATED");
            }
            return true;
        }
    }

    public class TopHash
    {
        public string Hash;
        public DateTime Date;
    }

    [Serializable]
    public class ProofBranch
    {

        public String hash;
        public decimal balance;
        [NonSerialized]
        public List<ProofUser> users;
        public Dictionary<string, ProofUser> usersDic;

        public ProofBranch(String h, decimal b, List<ProofUser> us)
        {
            hash = h;
            balance = b;
            users = us;
            usersDic = new Dictionary<string, ProofUser>();
            foreach (var user in us)
            {
                usersDic[user.name] = user;
            }
        }

        public static ProofBranch MakeBranch(String obj1, String obj2, decimal balance1, decimal balance2, List<ProofUser> us)
        {
            return new ProofBranch(Crypto.HashSHA1(obj1 + Crypto.decimaltoString(balance1) + obj2 + Crypto.decimaltoString(balance2)), balance1 + balance2, us);
        }

        public static ProofBranch MakeLeaf(ProofUser u, decimal balance)
        {
            List<ProofUser> us = new List<ProofUser>();
            us.Add(u);
            return new ProofBranch(u.GetHash(), balance, us);
        }
    }

    [Serializable]
    public class ProofBlindBranch
    {

        public String hash;
        public decimal balance;
        //[NonSerialized]
        public int layer;
        //[NonSerialized]
        public List<ProofBlindBranch> neighbours;

        public ProofBlindBranch(String h, decimal b, int l)
        {
            hash = h;
            balance = b;
            layer = l;
            neighbours = new List<ProofBlindBranch>();
        }

        public void AddNeighbour(ProofBlindBranch n)
        {
            neighbours.Add(n);
        }

        public ProofBlindBranch[] GetNeighbours()
        {
            return neighbours.ToArray();
        }

        public override bool Equals(object obj)
        {
            ProofBlindBranch other = obj as ProofBlindBranch;
            if (other == null)
                return false;
            return this.hash.Equals(other.hash) && this.balance.Equals(other.balance) && this.layer == other.layer;
        }
    }
}
