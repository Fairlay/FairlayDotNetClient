using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairlaySampleClient
{
    public partial class MatchedOrder : IComparable<MatchedOrder>
    {

        public enum MOState
        {
            MATCHED,
            RUNNERWON,
            RUNNERHALFWON,
            RUNNERLOST,
            RUNNERHALFLOST,
            MAKERVOIDED,
            VOIDED,
            PENDING,
            DECIMALRESULT,        

        }

        public decimal DecResult;
        public int R;
        public long ID;

        public MOState State;


        public decimal Price;
        public decimal Amount;
        public int MakerCancelTime;


        public DateTime ExcludedCreationTime
        {
            get
            {
                return new DateTime(ID);
            }
            set
            {

            }
        }

   

        public int CompareTo(MatchedOrder other)
        {
            return this.ID.CompareTo(other.ID);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Price.ToString(CultureInfo.InvariantCulture) + " ");
            sb.Append(Amount.ToString(CultureInfo.InvariantCulture) + " ");
            sb.Append(ExcludedCreationTime + " ");
            sb.Append(State + " ");

            return sb.ToString();
        }

     
    }
}
