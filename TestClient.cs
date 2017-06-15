using FairlaySampleClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FairlaySampleClient
{
    partial class TestClient
    {
        private Client _Client;
        public string SERVERKEY;
      
        ConfigDet Config;
        public bool RunCallbackListener;
        public string MyIP;
        public int MyPort;
        
        public TestClient(bool runcallback=false, string myip="", int myport=0)
        {

           Config = ConfigDet.ReadConfig();
            if(Config==null)
            {


                Config = new ConfigDet(true);
                Config.SERVERIP = "31.172.83.53";
                Config.PORT = 18017;
                Config.APIAccountID = 1;
                Config.WriteToFile();
               
            }
            MyIP = myip;
            MyPort = myport;

           
            RunCallbackListener = runcallback;
            if(runcallback)
            {
                Thread t1 = new Thread(waitForCallback);
                t1.Start();
            }
             
        }

      
         public string getPublicKey()
        {
            if (Config == null) return null;
            return Config.PublicRSAKey;
        }
        public bool init(int id=0)
        {
            if (id != 0 && Config.ID != id)
            {

                Config.ID = id;
                Config.WriteToFile();

            }
            if (Config.ID == 0)
            {
                return false;
            }
            _Client = new Client(Config.PrivateRSAKey, Config.PublicRSAKey, Config.ID, true);

            SERVERKEY = _Client.DoRequest(Config.SERVERIP, Config.PORT, REQ.GETPUBLICKEY, 0, null);
            if (SERVERKEY.Length > 20) return true;
           
            return false;

        }

        #region PublicGetRequests
        public bool VerifyProofOfReserves(decimal userBalance, string mustContain, string publicTopHash, decimal sumFunds)
        {

            var thash = makeReq(REQ.GETTOPHASH, "");
            if (String.IsNullOrEmpty(thash)) return false;
            
            var tophash = JsonConvert.DeserializeObject<TopHash>(thash);
            if (tophash == null || (publicTopHash != null && tophash.Hash != publicTopHash)) return false;
             var myid = makeReq(REQ.GETMYPROOFID, "");
            if(myid == null) return false;
            if (mustContain != null && !myid.Contains(mustContain)) return false;

            var proof = makeReq(REQ.GETPROOFOFRESERVES, "");
            if (proof == null) return false;
            bool verified = ProofUser.VerifyUserBranches(JsonConvert.DeserializeObject<ProofBlindBranch[]>(proof), myid, userBalance, tophash.Hash, sumFunds);
            return verified;

        }
        public long getServerTime()
        {

            long timestamp = -1;
            var ts = makeReq(REQ.GETSERVERTIME, "");

            if (ts != null) Int64.TryParse(ts, out timestamp);

            return timestamp;



        }

        public string[] getOrderbook(long mid)
        {
            var ts = makeReq(REQ.GETORDERBOOK, "" + mid);
            if (ts == null) return null;
            return ts.Split('~');
        }

        public List<MatchedOrder> getLatestbets5min()
        {
            var response = makeReq(REQ.LATESTBETS5MIN, "");
            if (response == null) return null;

            try
            {
                return JsonConvert.DeserializeObject<List<MatchedOrder>>(response);
            }
            catch (Exception) { }
            return null;
        }

        public List<MatchedOrder> getLatestbets60min()
        {
            var response = makeReq(REQ.LATESTBETS60MIN, "");
            if (response == null) return null;

            try
            {
                return JsonConvert.DeserializeObject<List<MatchedOrder>>(response);
            }
            catch (Exception) { }
            return null;
        }

        //Retrieve all settlements from the last 60 minutes.

        public List<SettleReq> getLatestSettlements()
        {
            var response = makeReq(REQ.LATESTSETTLEMENTS1H, "");
            if (response == null) return null;

            try
            {
                return JsonConvert.DeserializeObject<List<SettleReq>>(response);
            }
            catch (Exception) { }
            return null;
        }

        #endregion

        #region PrivateGetRequests
        public ReturnBalance getBalance()
        {
            var resp = makeReq(REQ.GETMYBALANCE, "");
            if (resp == null) return null;

            try
            {
                return JsonConvert.DeserializeObject<ReturnBalance>(resp);
            }
            catch (Exception) { }
            return null;
        }
       

        // Provide the UTC time in Ticks.
        public List<MUserStatement> getStatement(long sinceDate)
        {
            var answer = makeReq(REQ.GETSTATEMENT, sinceDate.ToString());

            if (answer == null) return null;

            var statement = JsonConvert.DeserializeObject<List<MUserStatement>>(answer);
            return statement;
        }

        // Provide the UTC time in Ticks.
        public List<MUserTransfer> getTransfers(long sinceDate)
        {
           
            var answer = makeReq(REQ.GETTRANSFERS, sinceDate.ToString());

            if (answer == null) return null;

            var transfers = JsonConvert.DeserializeObject<List<MUserTransfer>>(answer);
            return transfers;
        }
        //provide Dates in UTC
     
        public List<ReturnUOrder> getLongUOrderList(long time)
        {
            int start = 0;
            int maxitems = 1500;
            Dictionary<long, ReturnUOrder> allRet = new Dictionary<long, ReturnUOrder>();

            while (true)
            {
                int FromID = Math.Max(0, start - 10);
                int ToID = start + maxitems;

                //  var sfdx = placeOrder(3017402, 0, 0, 1.01m, 10, "");
                //  var x1 = makeReq(REQ.CHANGECLOSINGTIME, 3017402 + "|" + Util1.getUTCNow.AddHours(1));

                var ret = makeReq(REQ.GETUNMATCHEDORDERS, time + "|" + FromID + "|" + ToID);

                if (ret == null)
                {
                    System.Threading.Thread.Sleep(1000);
                    continue;
                }


                try
                {
                    var getm = JsonConvert.DeserializeObject<List<ReturnUOrder>>(ret);




                    foreach (var m in getm)
                    {
                        if (!allRet.ContainsKey(m._UserOrder.OrderID)) allRet.Add(m._UserOrder.OrderID, m);
                        else
                        {
                            allRet[m._UserOrder.OrderID] = m;

                        }
                    }

                    if (getm.Count < maxitems) break;
                    else start += maxitems;
                }
                catch (Exception) { break; }


            }


            return new List<ReturnUOrder>(allRet.Values);

        }


        public List<ReturnMOrder> getLongMOrderList(long time)
        {
            int start = 0;
            int maxitems = 1500;
            var allRet = new Dictionary<long, ReturnMOrder>();

            while (true)
            {
                int FromID = Math.Max(0, start - 10);
                int ToID = start + maxitems;

              
                var ret = makeReq(REQ.GETMATCHEDORDERS, time + "|" + FromID + "|" + ToID);

                if (ret == null)
                {
                    System.Threading.Thread.Sleep(1000);
                    continue;
                }


                try
                {
                    var getm = JsonConvert.DeserializeObject<List<ReturnMOrder>>(ret);




                    foreach (var m in getm)
                    {
                        if (!allRet.ContainsKey(m._UserOrder.OrderID)) allRet.Add(m._UserOrder.OrderID, m);
                        else
                        {
                            allRet[m._UserOrder.OrderID] = m;

                        }
                    }

                    if (getm.Count < maxitems) break;
                    else start += maxitems;
                }
                catch (Exception) { break; }


            }



            return new List<ReturnMOrder>(allRet.Values);

        }
    

        public List<ReturnMOrder> getMOrderList(long time)
        {

            var ret = makeReq(REQ.GETMATCHEDORDERS, time + "");

            try
            {
                return JsonConvert.DeserializeObject<List<ReturnMOrder>>(ret);
            }
            catch (Exception) { }


            return null;
        }

        public List<ReturnMOrder> getMOrderList(long time, long mid)
        {

            var ret = makeReq(REQ.GETMATCHEDORDERS, time + "|" + mid);

            try
            {
                return JsonConvert.DeserializeObject<List<ReturnMOrder>>(ret);
            }
            catch (Exception) { }


            return null;
        }
        #endregion

        #region ManageOrderRequests

        //Allows you to create, cancel and alter orders
        //Set Pri to 0  to cancel an order
        //Set Oid to -1 to create an order
        // Maximum allowed orders in one request: 50
        public bool changeOrders(REQChangeOrder[] coL)
        {
            if (coL.Length > 50) return false;

            var answL = sendChangeOrders(coL);
          
            if (answL == null)
            {
              

                    return false;

              
            }
            else
            {
                List<long> markets_to_cancel = new List<long>();
                for (int i = 0; i < coL.Length; i++)
                {
                    var co = coL[i];
                    string answ = answL[i];
                    if (answ.Contains("YError:Market Closed") || answ == "Order cancelled")
                    {
                        //order cancelled
                    }
                    else if (answ.Contains("YError"))
                    {
                        markets_to_cancel.Add(co.Mid);
                    }
                    else
                    {

                        try
                        {
                            var ro = JsonConvert.DeserializeObject<UnmatchedOrder>(answ);
                            //order created or changed
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                    }
                }


                cancelOrdersOnMarket(markets_to_cancel);
               

            }
            return true;
        }

        public string[] sendChangeOrders(REQChangeOrder[] coL)
        {
           
            string send = JsonConvert.SerializeObject(coL);
            string ret =makeReq(REQ.CHANGEORDERSV2,send);
            //  Utilities.append(send + " ---  \r\n --- " + ret);
            if (ret == null) return null;


            try
            {
                return JsonConvert.DeserializeObject<string[]>(ret);
            }
            catch (Exception)
            {
            }

            return null;
        }


    
        //please state a reason for the cancellation if possible, which is forwarded to the other party
        // 0:  not provided
        // 1:  other reason
        // 
        // 10: line changed
        // 11: market offline
        // 12: market  closed
        // orderID represents the ID of the MATCHED order.
        public bool makervoidMatchedOrder(long mid, int ruid, long orderID, int reason)
        {
            var cancelResult = makeReq(REQ.CANCELMATCHEDORDER, mid + "|" + reason + "|" + +ruid + "|" + orderID);
            if (cancelResult == null) return false;

            if (cancelResult.Contains("Cancellation successful")) return true;
            if (cancelResult.Contains("Order will be cancelled after Activation")) return true;
            if (cancelResult.Contains("Order does not exist")) return true;

            return false;
        }


        //reason can be set to 0 if the order is confirmed
        // please call this when possible for faster confirmation for the other party.
        // orderID represents the ID of the MATCHED order.
        // set the am(ount) to -1  to fully confirm tbe order, if only a part of it shall be confirmed, define the amount accordingly.
        public bool confirmMatchedOrder(long mid, int ruid, long orderID, int reason, decimal am = -1m)
        {
            var cancelResult = makeReq(REQ.CONFIRMMATCHEDORDER, mid + "|" + reason + "|" + ruid + "|" + orderID + "|" + am.ToString(CultureInfo.InvariantCulture));

            if (cancelResult == null) return false;

            if (cancelResult.Contains("Confirmation successful")) return true;
            if (cancelResult.Contains("Order could not be confirmed")) return false;

            return false;
        }
     
        public bool cancelAllOrders()
        {
            var resp = makeReq(REQ.CANCELALLORDERS, "");
            while (resp == null || !resp.Contains("cancelled"))
            {
                return false;
            }
            return true;

        }

        public bool cancelOrdersOnMarket(List<long> mids)
        {
            if (mids == null || mids.Count == 0) return true;
            var resp = makeReq(REQ.CANCELORDERSONMARKETS, JsonConvert.SerializeObject(mids));
            while (resp == null || !resp.Contains("cancelled"))
            {
                return false;
            }
            return true;

        }

#endregion
        #region LMSR
       
        // see the LMSR object for documentation
        //to disable a MM, set Enabled to false;
        public LMSR setLMSR(LMSR lmsr)
        {
            if (lmsr == null) return null;
            var answer = makeReq(REQ.SETMARKETMAKER1, JsonConvert.SerializeObject(lmsr));

            if (answer == null || answer.StartsWith("XError")) return null;

            try
            {
                return JsonConvert.DeserializeObject<LMSR>(answer);
                 
            }
            catch (Exception)
            {

                return null;
            }
          
        }

       
        public List<LMSR> getAllLMSRs()
        {
            var answer = makeReq(REQ.GETMARKETMAKER, "-1");
            if (answer == null || answer.StartsWith("XError")) return null;

            try
            {
                return JsonConvert.DeserializeObject<List<LMSR>>(answer);

            }
            catch (Exception)
            {

                return null;
            }

        }
        #endregion

        #region MarketRelatedRequests

        public long createMarket(MarketX m)
        {
            var mid = makeReq(REQ.CREATEMARKET, JsonConvert.SerializeObject(m));
            long Mid;
            bool succ = Int64.TryParse(mid, out Mid);
            if (!succ) return 0;
            return Mid;

        }

        public bool ChangeMarketTimes(long mid, DateTime closD, DateTime setlD)
        {
            var trq = new ChangeTimeReq();
            trq.MID = mid;
            trq.ClosD = closD;
            trq.SetlD = setlD;


            var answer = makeReq(REQ.CHANGETIMEREQUEST, JsonConvert.SerializeObject(trq));
            if (answer == null) return false;
            return true;
        }
        //Settle a market you created. 
        //The full turnover of the market will be deducted from your Available Balance for 3 to 4 days.
        public bool settleMarket(long mid, int runnerid, bool win=true, bool half=false)
        {
            var xf = new SettleReq();
            xf.Mid = mid;
            xf.Runner = runnerid;
            xf.Win = win? 1:2;
            xf.Half = half;
            var respdd = makeReq(REQ.SETTLEREQUEST, JsonConvert.SerializeObject(xf));
            if (respdd == null || !respdd.Contains("settled")) return false;
            return true;
        }

        #endregion

        #region AccountRelatedRequests

        //To delete an existing API Account, set PublicRSAKeyXML to "DELETE"  and send the request from the native API Account #0
        //The transferLimit should only be set for TransferOnly API Accounts. If the transferLimit is set to 0, it means that there is no limit.  
        public bool registerAPIAccount(int apiAccount, string PublicRSAKeyXML, bool isReadOnly, bool isTransferOnly, decimal dailyTransferLimitmBTC = 0)
        {
            string req = string.Format("{0}|{1}|{2}|{3}|{4}", apiAccount, PublicRSAKeyXML, isReadOnly, isTransferOnly, dailyTransferLimitmBTC.ToString(CultureInfo.InvariantCulture));


            var response = makeReq(REQ.REGISTERAPI2, req);
            if (response == "New APIUser Added") return true;
            return false;
        }
        public bool setPublicUserName(string name)
        {

            var answer = makeReq(REQ.SETSCREENNAME, "" + name);
            if (answer != null && answer == "success") return true;


            return false;
        }


        //if you are the maker of a bet, you will be notified via a TCP request with all details about the matched order
        //make sure you listen on the selected port and ip and that your firewall is not blocking the requests
        //each callback costs 1 request
        //the callbacks will automatically stop if they fail more than 100 times or if you do not have enough requests left
        //Special Permission dependent on the node you are connecting to are currently required
        //make sure the runner market you are betting on allows you to cancel (makervoid) bets after they are matched  (VisDelay must be >0)
        //also, you must set makerCT of your UnmatchedOrder to a value less or equal to the VisDelay of the Runner Market you are betting on
        //you are than able to void any MatchedOrder within "makerCT" milliseconds. After that the MatchedOrder will go from State PENDING to MATCHED.

      
        public bool setCallbackIP(string ip, int port)
        {
            var answer = makeReq(REQ.SETCALLBACKIP, ip + ":" + port);      
            if (answer != null && answer == "success") return true;
          

            return false;
        }


        private void waitForCallback()
        {

            TcpListener myList = new TcpListener(IPAddress.Parse("0.0.0.0"), MyPort);

            try
            {
                myList.Start();

            }
            catch (Exception)
            {
                //alert
                return;
            }

            var response2 = setCallbackIP(MyIP, MyPort);

            while (RunCallbackListener)
            {
                Socket s = myList.AcceptSocket();
                int timeout = 3000;

                s.ReceiveTimeout = timeout;
                s.SendTimeout = timeout;

                byte[] b = new byte[100000];
                int k = 0;

                try
                {
                    k = s.Receive(b);
                    string clientreq = Encoding.UTF8.GetString(b, 0, k);
                    s.Close();

                    if (clientreq.EndsWith("<ENDOFDATA>"))
                    {
                        clientreq = clientreq.Remove(clientreq.Length - 11);

                        var clreqA = clientreq.Split('|');

                        string sigString = clientreq.Substring(clientreq.IndexOf("|") + 1);
                        var legimate = Util1.VerifyData(sigString, clreqA[0], SERVERKEY);

                        var moString = clientreq.Substring(clreqA[0].Length + clreqA[1].Length + clreqA[2].Length + 3);

                        if (!legimate)
                        {
                            //send alert

                            continue;
                        }


                        var mro = JsonConvert.DeserializeObject<ReturnMOrder>(moString);

                        //work with the matched order

                        //confirm it 
                        confirmAndSetPosition(mro);

                        //confirm it partly (10mBTC)                      
                        //confirmAndSetPosition(mro, 0, 10m);


                        //or makervoid (cancel) it for some reason
                        //confirmAndSetPosition(mro, 1, 0m);

                    }

                }
                catch (Exception ex)
                {
                    // send an alert...
                }


            }
            myList.Stop();


        }


        private bool confirmAndSetPosition(ReturnMOrder mro, int reason = 0, decimal am = -1)
        {
            if (am == 0)
            {

                var cancelResult = makervoidMatchedOrder(mro._UserOrder.MarketID, mro._UserOrder.RunnerID, mro._UserOrder.OrderID, reason);

                return cancelResult;

            }
            else
            {


                var confirmResult = confirmMatchedOrder(mro._UserOrder.MarketID, mro._UserOrder.RunnerID, mro._UserOrder.OrderID, 0, am);

                if (confirmResult)
                {

                    #region adjustInternalPosition
                    //int ruLength = ?;
                    //var position = new decimal[ruLength];

                    //if (mro._UserOrder.BidOrAsk == 0)
                    //{

                    //    for (int uix = 0; uix < ruLength; uix++)
                    //    {
                    //        if (uix != mro._UserOrder.RunnerID)
                    //        {
                    //            position[uix] += Math.Round(am, 2);
                    //        }
                    //    }

                    //    position[mro._UserOrder.RunnerID] -= Math.Round((mro._MatchedOrder.Price - 1) * am, 2);
                    //}
                    //else
                    //{

                    //    for (int uix = 0; uix < ruLength; uix++)
                    //    {
                    //        if (uix != mro._UserOrder.RunnerID)
                    //        {
                    //            position[uix] -= Math.Round(am, 2);
                    //        }
                    //    }

                    //    position[mro._UserOrder.RunnerID] += Math.Round((mro._MatchedOrder.Price - 1) * am, 2);

                    //}
                    #endregion
                }

                return confirmResult;
            }
        }
  
        public bool transferFunds(int to, string reference, int ttype, decimal amountMBTC)
        {
            var userTransfer = new MUserTransfer(Config.ID, to, reference, ttype,amountMBTC);
            var answer = makeReq(REQ.TRANSFERFUNDS, JsonConvert.SerializeObject(userTransfer));

            if (answer == null) return false;

         //   var transfer = JsonConvert.DeserializeObject<MUserTransfer>(answer);
            return true;   
        }
        // you will receive mBTCs in exchange for fairlay credits if you send funds to the user 111111 for example.
        // withdrawal fees are subject to change.
        public bool doWithdrawal(string address, decimal amountMBTC)
        {
            return transferFunds(111111, address, 2, amountMBTC);
        }

        //Every unmatched order of your account will be cancelled after X milliseconds without any request of any API Account.
        public bool setAbsenceCancelPolicy(long mseconds)
        {

            var cancelResult = makeReq(REQ.SETABSENCECANCELPOLICY, mseconds.ToString());
            if (cancelResult == null) return false;

            if (cancelResult.Contains("success")) return true;
            return false;
        }

        //Warning: sets your current API Account to read only. This cannot be undone!  Never set your native api account #0 to read-only
        // as you won't be able to create another API Account;
        public bool setAPIAccountToReadOnly()
        {

            var cancelResult = makeReq(REQ.SETREADONLY, "");
            if (cancelResult == null) return false;

            if (cancelResult.Contains("success")) return true;
            return false;
        }

        //Force Nonce with every Requests - All GET-requests do not require a nonce by default
        public bool setForceNonce(bool forceNonce)
        {

            var cancelResult = makeReq(REQ.SETFORCENONCE, "" + forceNonce);
            if (cancelResult == null) return false;

            if (cancelResult.Contains("success")) return true;
            return false;
        }

        //For market makers.   If you set ForceConfirm to false (default)  all bets that are matched against 
        // your existing orders will go into the state MATCHED after the MAKERCANCELTIME has passed and no confirmation or cancellation was sent by you.
        // If set to true, your orders will go into the state MAKERVOIDED after the MAKERCANCELTIME has passed and no confirmation or cancellation was sent by you.

        public bool setForceConfirm(bool force)
        {

            var cancelResult = makeReq(REQ.SETFORCECONFIRM, "" + force);
            if (cancelResult == null) return false;

            if (cancelResult.Contains("success")) return true;
            return false;
        }

        public bool setSettleDelegates(long[] accountIds)
        {

            var cancelResult = makeReq(REQ.SETSETTLEDELEGATES, "" + JsonConvert.SerializeObject(accountIds));
            if (cancelResult == null) return false;

            if (cancelResult.Contains("success")) return true;
            return false;
        }

        #endregion

        #region Private

        private string makeReq(int reqID, string param1, int maxms = 2000)
        {
            int reqIDsend = reqID + 1000*Config.APIAccountID;
            string param = reqIDsend + "|" + param1;

            string ret = _Client.DoRequestAndVerify(Config.SERVERIP, Config.PORT, param, SERVERKEY);
            int triesunav = 0;
            while (ret != null && ret.Contains("XError: Service unavailable"))
            {
                System.Threading.Thread.Sleep(5000);
                ret = _Client.DoRequestAndVerify(Config.SERVERIP, Config.PORT, param, SERVERKEY);
                triesunav++;
                if (triesunav > 10) break;

            }
           
            if (ret == null || ret.Contains("XError:"))
            {
                 Console.WriteLine("Error:" + ret);
                return null;
            }
            return ret;
        }



        #endregion
    }
}
