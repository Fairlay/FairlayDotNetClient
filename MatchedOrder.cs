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


        
        //when two open orders are matched, a Matched Order is created in the PENDING state.  
        // If the maker of the bet cancels his bet within a certain time period (usually 0, 3 or 6 seconds depending on the market) 
        // the bet goes into the state MAKERVOIDED and is void.
        // When a market is settled the orders go in one of the settled states VOID, WON, HALFWON, LOST or HALFLOST.  
        // Decimal market go into the state DECIMALRESULT while the settlement value DecResult will be set.

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
            DECIMALRESULTTOBASE

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

        public static decimal getOrderLiability(int bidorask, decimal minmaxAmount, decimal price, decimal amount, MarketX.SettleType settlT)
        {
            if (settlT == MarketX.SettleType.BINARY)
            {
                if (bidorask == 1)
                {
                    return amount;
                }
                else
                {
                    return amount * (price - 1);
                }
            }
            else if (settlT == MarketX.SettleType.DECIMAL)
            {

                if (bidorask == 0)
                {
                    return (price - minmaxAmount) * amount;
                }
                else
                {
                    return (minmaxAmount - price) * amount;

                }
            }
            else if (settlT == MarketX.SettleType.DECIMALTOBASE)
            {

                if (bidorask == 0)
                {
                    return (price / minmaxAmount - 1) * amount;
                }
                else
                {
                    return (1 - price / minmaxAmount) * amount;

                }
            }
            return decimal.MaxValue;



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
