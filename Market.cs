using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Globalization;

namespace FairlaySampleClient
{
    public static class CATEGORY
    {
        public const int SOCCER = 1;
        public const int TENNIS = 2;
        public const int GOLF = 3;
        public const int OTHERSPORTS = 4;
        public const int RUGBYUNION = 5;
        public const int BOXING = 6;
        public const int HORSERACING = 7;
        public const int MOTORSPORT = 8;
        public const int SPECIAL = 10;
        public const int RUGBYLEAGUE = 11;
        public const int BASKETBALL = 12;
        public const int AMERICANFOOTBALL = 13;
        public const int BASEBALL = 14;
        public const int HOCKEY = 30;


        public const int POLITICS = 15;
        public const int FINANCIAL = 16;
        public const int GREYHOUND = 17;
        public const int VOLLEYBALL = 18;
        public const int HANDBALL = 19;
        public const int DARTS = 20;
        public const int BANDY = 21;
        public const int WINTERSPORTS = 22;
        public const int BOWLS = 24;
        public const int POOL = 25;
        public const int SNOOKER = 26;
        public const int TABLETENNIS = 27;
        public const int CHESS = 28;
        public const int FUN = 31;
        public const int ESPORTS = 32;
        public const int INPLAY = 33;
        public const int RESERVED4 = 34;
        public const int MIXEDMARTIALARTS = 35;
        public const int RESERVED6 = 36;
        public const int RESERVED = 37;
        public const int CYCLING = 38;
        public const int RESERVED9 = 39;
        public const int BITCOIN = 40;
        public const int BADMINTON = 42;



    }

    public partial class MarketX :  IComparable<MarketX>
    {
      

        public enum SettleType
        {
            BINARY,
            DECIMAL,
            DECIMALTOBASE

        }
        public enum MarketType
        {
            M_ODDS,
            OVER_UNDER,
            OUTRIGHT,
            GAMESPREAD,
            SETSPREAD,
            CORRECT_SCORE,
            FUTURE,
            BASICPREDICTION,
            RESERVED2,
            RESERVED3,
            RESERVED4,
            RESERVED5,
            RESERVED6

            

        }

        public enum MarketPeriod
        {
            UNDEFINED,
            FT,
            FIRST_SET,
            SECOND_SET,
            THIRD_SET,
            FOURTH_SET,
            FIFTH_SET,
            FIRST_HALF,
            SECOND_HALF,
            FIRST_QUARTER,
            SECOND_QUARTER,
            THIRD_QUARTER,
            FOURTH_QUARTER,
            FIRST_PERIOD,
            SECOND_PERIOD,
            THIRD_PERIOD,
           
        }

        public enum StatusType
        {
            ACTIVE,
            INPLAY,
            SUSPENDED,
            CLOSED,
            SETTLED,
            CANCELLED

        }

        public string OrdBStr;

        
        public string Comp { get; set; }
        public long ID;
       
        public string Descr { get; set; }

        public string Title { get; set; }
        public int CatID { get; set; }
        public DateTime ClosD { get; set; }
        public DateTime SettlD { get; set; }
        
       
        public StatusType Status { get; set; }
        public Runner[] Ru { get; set; }

        public MarketType _Type { get; set; }
        public MarketPeriod _Period { get; set; }
        public SettleType SettlT { get; set; }

        public decimal Comm { get; set; }
        public long PrivCreator { get; set; }


        //Determines all allowed Settler for this market and if they are trusted or not. 
        // Untrusted settlers may settle the market, but have to put in the entire volume as Security deposit for 3 days
        // In order for a market to be listed as safe on Fairlay.com, the user "1" (Fairlay) has to be put in as trusted settler 
        // and the user 777889 has also to be put in as settler for faster settlement.
        // all other settlers have to be untrusted
        // Any defined settler may determine "settle delegates" who may settle markets in their name, but whose settlement can be overturned.
        
        
        public Dictionary<long, bool> Settler { get; set; }

        //Determines all users, who receive the commission of the market. 
        // In order to be listed on the Fairlay.com website, the user 1 (Fairlay) has to receive at least 50%  (0.5)
        public Dictionary<long, decimal> ComRecip { get; set; }
     


        public DateTime ExcludedCreationTime
        {
            get
            {
               return new DateTime(2014,1,1).AddMilliseconds(ID);
            }
            set
            {
                
            }
        }
        public DateTime LastCh {
            get
            {
                return lastHardChange;
            }
            set
            {
                lastHardChange = value;
                lastSoftChange = value;

            }
        
        }

        public DateTime LastSoftCh
        {
            get
            {
                return lastSoftChange;
            }
            set
            {
                lastSoftChange = value;

            }

        }

        public double Pop;
        public decimal Margin;
        public decimal ExcludedMargin200;
       

        private DateTime lastHardChange;
        private DateTime lastSoftChange;

        public MarketX()
        {

        }

       

        public MarketX(MarketType mtype, MarketPeriod mper,  string title, string competition, int creator, string description, int category, DateTime closing, DateTime settling, string[] runnerNames, bool inplay=false ,decimal commission=0.02m)
        {

            _Period = mper;
            _Type = mtype;
            Title = Util1.RemoveDiacritics(title);
            PrivCreator = creator;
            Descr = Util1.RemoveDiacritics(description);
            ClosD = closing;
            SettlD = settling;
            CatID = category;
            Comp = Util1.RemoveDiacritics(competition);
            Comm = commission;


            Settler = new Dictionary<long, bool>(3);
            Settler[1] = true;
            Settler[777889] = false;
            Settler[creator] = false;


            if (commission > 0)
            {
                ComRecip = new Dictionary<long, decimal>();
                ComRecip[1] = 0.5m;
                ComRecip[creator] = 0.5m;
            }

          
            if (inplay)
            {
                 Status = StatusType.INPLAY;
            }
            else Status = MarketX.StatusType.ACTIVE;
            Ru = new Runner[runnerNames.Length];
            for (int i = 0; i < runnerNames.Length; i++)
            {
                Ru[i] = new Runner(runnerNames[i],  6000);
            }

        }


        public int CompareTo(MarketX other)
        {
            return this.ID.CompareTo(other.ID);

        }

        public static int CompareClosingTime(MarketX a, MarketX b)
        {
            if (a.ClosD == b.ClosD)
            {

               return  a.CompareTo(b);

            }

            return a.ClosD.CompareTo(b.ClosD);

        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Comp + " ");
            sb.Append(Title + " ");
            sb.Append(_Period + " ");
            sb.Append(_Type + " ");
            sb.Append(Margin.ToString(CultureInfo.InvariantCulture) + " ");
            sb.Append(Ru[0].Name + " ");
            sb.Append(ClosD + " ");
            sb.Append(OrdBStr + " ");

            return sb.ToString();
        }

        public string ToString(int rid)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(Comp + " ");
            sb.Append(Title + " ");
            sb.Append(_Period + " ");
            sb.Append(_Type + " ");
            sb.Append(Margin.ToString(CultureInfo.InvariantCulture) + " ");
            sb.Append(Ru[rid].Name + " ");
            sb.Append(ClosD + " ");
            sb.Append(OrdBStr + " ");

            return sb.ToString();


        }

    }
}
