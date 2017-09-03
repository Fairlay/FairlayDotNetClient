using System;

namespace FairlayDotNetClient.Private.Datatypes {
	/// <summary>
	/// LMSR documentation
	/// Example {"UserID":1000001,"MarketID":61659266392,"Runner":5,"Enabled":true,"InitShareLimit":350.0,"B":1300.0,"CancelAll":"2016-06-05T13:34:56", "ShareStop":1400.0,"InitProb":[0.3,0.1,0.2,0.2,0.2],"DiminishBack":[0.01,0.01,0.01,0.0,0.02],"DiminishLay":[0.0,0.0,0.0,0.01,0.01],"coolOffSeconds":36000.0,"coolOffFactor":4.0}
	/// Default values {"UserID":[userid],"MarketID":61659266392,"Runner":5,"Enabled":true,"InitShareLimit":300.0,"B":1000.0,"CancelAll":"2016-06-05T13:34:56", "ShareStop":9999.0,"InitProb":[0.2,0.2,0.2,0.2,0.2],"DiminishBack":[0.00,0.00,0.00,0.0,0.00],"DiminishLay":[0.01,0.01,0.01,0.01,0.01],"coolOffSeconds":1.0,"coolOffFactor":2.0}
	/// UserID: must match your userid
	/// MarketID:  must be provided
	/// Runner:  # of Runners / must match the # of runners of the market
	/// Enabled: must be set to true
	/// InitShareLimit(must be > 1):    Shares that are offered in one order.Stake + Winnings from
	/// one order are 350mBTC
	/// B (must be > 10):    ~ is about the same as the maxium possible loss of the market maker
	/// CancelAll(must be set to a future date):   Date where the market maker stops.Set to year 2100
	/// if the mm should run forever
	/// ShareStop (must be > 1):   amount of exposure in shares before the market maker stops. Should
	/// be set higher than  B in regular cases.
	/// InitProb:   the initial probability estimation for all runners
	/// DiminishBack (must be non-negative):   In general the LMSR market maker runs on 0% margin,
	/// i.e.it doesn't make any profit.  If more margin should be added, you can worsen the odds for
	/// each bid orders for every runner.  0.01 worsens bid odds from  80% to 81%  (or 1.25 to 1.2345)
	/// for example. 
	/// DiminishLay: same for all ask orders.
	/// cool off adds temporary additional margin to markets with increased activity and should be
	/// applied to markets that can have exogenous shocks or where real probabilities can deviate from
	/// the initial probability distribution.
	/// coolOffFactor(must be >=1):    if set to 4.0 the odds will worsen 4.0 times more than expected
	/// from the usual lmsr market maker.
	/// coolOffSeconds(must be >=1):    The time after which the coolOff period will be over. If set
	/// to 36000 the additional market margin will reduce step by step over an period of 10 hours.
	/// If no cool off is required, set coolOffSeconds to 1.
	/// </summary>
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
}