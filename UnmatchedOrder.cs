using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairlaySampleClient
{
    public partial class UnmatchedOrder : IComparable<UnmatchedOrder>
    {

        public enum UOState
        {
            Active,
            Cancelled,
            Matched,
            MatchedAndCancelled

        }


        public enum Type
        {
            MAKERTAKER,
            MAKER,
            TAKER
        }
        public decimal Price { get; set; }
        public decimal RemAmount { get; set; }

        public Type _Type;



        public decimal ExcludedWinnings
        {
            get
            {
                return (Price - 1) *RemAmount;
            }

        }

        public UOState State { get; set; }

        public int BidOrAsk { get; set; }
        
    
        public decimal PrivAmount { get; set; }



        public long PrivID;
      
        public string PrivSubUser { get; set; }

      
        public int makerCT { get; set; }

        public long PrivUserID;

        public DateTime ExcludedCreationTime
        {
            get
            {
                return new DateTime(PrivID);
            }
            set
            {

            }
        }

        public UnmatchedOrder(bool layliability, long uid, int bidorask, decimal price, decimal amount, Type typ, string subUser, int mCTime)
        {
            PrivUserID = uid;
        
            BidOrAsk = bidorask;
            Price = price;
            PrivAmount = amount;
            if (layliability) PrivAmount = Math.Round(amount / (price - 1), 5);
            RemAmount = amount;
            PrivSubUser = subUser;

            makerCT = mCTime;
            _Type = typ;

            State = UOState.Active;
           
            
        }

        public int CompareTo(UnmatchedOrder other)
        {
            
            if (this == null) return 1;
            if (other == null) return -1;

            if (this.Price != other.Price)
            {
                if (BidOrAsk==1)
                {
                    return this.Price.CompareTo(other.Price);
                }
                else
                {
                    return other.Price.CompareTo(this.Price);
                }
            }
            else return this.PrivID.CompareTo(other.PrivID);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(BidOrAsk + " ");
            sb.Append(Price.ToString(CultureInfo.InvariantCulture) + " ");
            sb.Append(RemAmount.ToString(CultureInfo.InvariantCulture) + " ");
            sb.Append(PrivSubUser + " ");
       
            return sb.ToString();
        }

    }



}
