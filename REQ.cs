using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairlaySampleClient
{
    public class ChangeTimeReq
    {
        public long MID;
        public DateTime? ClosD;
        public DateTime? SetlD;
    }


    public class SettleReq
    {
        public long Mid;
        public int Runner;
        public bool Unsettle;
        public int Win;  //The selected Runner either  won (1)  or lost (2)
        public bool Half;  // Needed for example for markets like   Over/Under 2.25   which is Over / Under Markets 2.5  & 2.0     
        public decimal Dec;  // For Decimal Markets
        public decimal ORed;  // Odds Reduction   (for Horse Racings)
        public int[] VoidRunners; // Voiding for Horse Racing Settlements
        public DateTime Executed;

    }

    public class SetRedReq
    {
        public long Mid;
        public int Runner;
        public long Oid;
        public decimal Red;

    }

    public class REQChangeOrder
    {
        
        public long Mid;
        //RunnerID  0:1st runner; 1: 2nd runner; ....
        public int Rid;
        // Order ID  (should be set to -1 if no old order shall be replaced)
        public long Oid;
        //Amount in mBTC
        // In case of ask orders this amount represents the liability, in case of bid orders this amount represents the possible winnings. 
        public decimal Am;
        // Set the price to 0  if you like to cancel an order and not create a new one.
        public decimal Pri;
        //Text for your own reference
        public string Sub;
        public UnmatchedOrder.Type Type;
        // Must be 0 for Bid Orders  and 1 for Ask.  Ask means that you bet on the outcome to happen.
        public int Boa;
        //This is the Maker cancel time.  How much time you have as maker of the bet to void the bet after the intial matching. 
        // should be set to 0 by default. Is only interesting for professional market makers. Learn more about it by reading how our matching process works in the README.rst
        public int Mct;
        //Set this to true if you place a bid order on a binary market and you like to have the Amount as Liability of the Order.
        public bool LayAsL;
        //Order will be cancelled  at this time in Ticks in UTC, set above 0 to use  if  it's below the current time in ticks, the order won't be created.
        public long CAt;
        //Response   returns either an json encoded UnmatchedOrder object or an XError
        public string Res;

    }

    public class ReturnBalance
    {
        public decimal PrivReservedFunds;
        public decimal MaxFunds;
        public decimal PrivUsedFunds;
        public decimal AvailableFunds;
        public decimal SettleUsed;
        public decimal CreatorUsed;
        public int RemainingRequests;

    }

    /*  LMSR documentation
     * 
     * example {"UserID":1000001,"MarketID":61659266392,"Runner":5,"Enabled":true,"InitShareLimit":350.0,"B":1300.0,"CancelAll":"2016-06-05T13:34:56", "ShareStop":1400.0,"InitProb":[0.3,0.1,0.2,0.2,0.2],"DiminishBack":[0.01,0.01,0.01,0.0,0.02],"DiminishLay":[0.0,0.0,0.0,0.01,0.01],"coolOffSeconds":36000.0,"coolOffFactor":4.0}

Default values
{"UserID":[userid],"MarketID":61659266392,"Runner":5,"Enabled":true,"InitShareLimit":300.0,"B":1000.0,"CancelAll":"2016-06-05T13:34:56", "ShareStop":9999.0,"InitProb":[0.2,0.2,0.2,0.2,0.2],"DiminishBack":[0.00,0.00,0.00,0.0,0.00],"DiminishLay":[0.01,0.01,0.01,0.01,0.01],"coolOffSeconds":1.0,"coolOffFactor":2.0}

UserID: must match your userid
MarketID:  must be provided
Runner:  # of Runners / must match the # of runners of the market
Enabled: must be set to true
InitShareLimit (must be > 1):    Shares that are offered in one order.  Stake + Winnings from one order are 350mBTC
B (must be > 10):    ~ is about the same as the maxium possible loss of the market maker
CancelAll (must be set to a future date):   Date where the market maker stops. Set to year 2100 if the mm should run forever
ShareStop (must be > 1):   amount of exposure in shares before the market maker stops.  Should be set higher than  B in regular cases.
InitProb:   the initial probability estimation for all runners
DiminishBack (must be non-negative):   In general the LMSR market maker runs on 0% margin, i.e. it doesn't make any profit.  If more margin should be added, you can worsen the odds for each bid orders for every runner.  0.01 worsens bid odds from  80% to 81%  (or 1.25 to 1.2345)  for example. 
DiminishLay:   same for all ask orders. 

cool off adds temporary additional margin to markets with increased activity and should be applied to markets that can have exogenous shocks or where real probabilities can deviate from the initial probability distribution. 

coolOffFactor (must be >=1):    if set to 4.0 the odds will worsen 4.0 times more than expected from the usual lmsr market maker.    
coolOffSeconds (must be >=1):    The time after which the coolOff period will be over. If set to 36000 the additional market margin will reduce step by step over an period of 10 hours. 
If no cool off is required, set coolOffSeconds to 1.
     */

    public class LMSR
    {

        public long UserID;
        public long MarketID;
        public int Runner;

        public bool Enabled;
        public double InitShareLimit;
        public double B;

        public DateTime CancelAll;        
        public double ShareStop;

        public double[] DiminishBack;
        public double[] DiminishLay;

        public double[] InitProb;        

        public double coolOffSeconds;
        public double coolOffFactor;

    }
    public static partial class REQ
    {
        public const string GETPUBLICKEY = "GETPUBLICKEY";
        
        public const int GETORDERBOOK = 1;  
        public const int GETSERVERTIME = 2; 
        public const int GETMARKET = 6; 
        public const int CREATEMARKET = 11;

        public const int CANCELORDERSONMARKET = 10; 
        public const int CANCELALLORDERS = 16; 
        public const int CANCELORDERSONMARKETS = 83; 
   

        public const int SETABSENCECANCELPOLICY = 43; 
        public const int SETFORCENONCE = 44;  
        public const int GETUNMATCHEDORDERS = 25;
        public const int GETMATCHEDORDERS = 27;
        public const int REGISTERAPI2 = 123;  
        public const int GETMYBALANCE = 22;

        public const int SETREADONLY = 49;

        public const int GETTOPHASH = 41; 
        public const int GETPROOFOFRESERVES = 42;
        public const int GETMYPROOFID = 50;

        public const int CANCELMATCHEDORDER = 9;  
        public const int CONFIRMMATCHEDORDER = 88;

        public const int TRANSFERFUNDS = 81;
        public const int GETTRANSFERS = 82; 
        public const int CHANGETIMEREQUEST = 84; 
     
        public const int GETSTATEMENT = 85;
        public const int SETTLEREQUEST = 86;

        public const int LATESTBETS5MIN = 101;
        public const int LATESTBETS60MIN = 102;
        public const int SETCALLBACKIP = 103;
        public const int SETFORCECONFIRM = 105; 
        public const int LATESTSETTLEMENTS1H = 106;
        public const int SETSCREENNAME = 46;
        public const int SETSETTLEDELEGATES = 107;

        public const int AMENDDESCRIPTION = 55; 
        public const int GETMARKETMAKER = 70; 
        public const int SETMARKETMAKER1 = 73;

        public const int GETAPIACCOUNTS = 108; // retrieves all registered API accounts  as Dictionary<int, MAPIUser>. No parameter required.
        public const int CHANGEORDERSV2 = 109;  //long MarketID|int  RunnerID|long OrderID| decimal Price| decimal amount
   
    }


 }
