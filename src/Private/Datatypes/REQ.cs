namespace FairlayDotNetClient.Private.Datatypes
{
	/// <summary>
	/// https://fairlay.com/api
	/// </summary>
	public static class REQ
	{
		public const string GETPUBLICKEY = "GETPUBLICKEY";
		public const int GETORDERBOOK = 1;
		public const int GETSERVERTIME = 2;
		public const int GETMARKET = 6;
		public const int GETMARKETS = 7; // TODO finish, currently using public api
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
		public const int CREATEORDER = 62;
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
		/// <summary>
		/// Retrieves all registered API accounts  as Dictionary<int, MAPIUser>. No parameter required.
		/// </summary>
		public const int GETAPIACCOUNTS = 108;
		/// <summary>
		/// long MarketID|int  RunnerID|long OrderID| decimal Price| decimal amount
		/// </summary>
		public const int CHANGEORDERSV2 = 109;
		public const int GETMYBALANCEV2 = 122;
		public const int GETCURRENCIES = 123;
		public const int ADDORCHANGECURRENCY = 124;
	}
}