using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairlaySampleClient
{

    public class REQChangeOrder
    {
        public long Mid;
        public int Rid;
        public long Oid;
        public decimal Am;
        public decimal Pri;
        public string Sub;
        public UnmatchedOrder.Type Type;
        public int Boa;
        public int Mct;

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
    

    public static partial class REQ
    {
        public const string GETPUBLICKEY = "GETPUBLICKEY";
        
        public const int CHANGEORDERSMAKER = 61; 
      
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
        public const int REGISTERAPI = 23;  
        public const int GETMYBALANCE = 22;
    
       
    }


    public static class CATEGORY
    {
        public const int SOCCER = 1;
        public const int TENNIS = 2;
        public const int GOLF = 3;
        public const int CRICKET = 4;
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
}
