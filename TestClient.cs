using FairlaySampleClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairlaySampleClient
{
    partial class TestClient
    {
        private Client _Client;
        public string SERVERKEY;
      
        ConfigDet Config;

        
        public TestClient()
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

             
        }

    
        public string getPublicKey()
        {
            if (Config == null) return null;
            return Config.PublicRSAKey;
        }
        public bool init(int id=0)
        {
            if (id != 0)
            {
                if (Config.ID != id)
                {
                    Config.ID = id;
                    Config.WriteToFile();
                }
                else  Config.ID = id;
              
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

        #region ManageOrders

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
            string ret =makeReq(REQ.CHANGEORDERSMAKER,send);
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

        #region OtherRequests

        public long createMarket(MarketX m)
        {
            var mid = makeReq(REQ.CREATEMARKET, JsonConvert.SerializeObject(m));
            long Mid;
            bool succ = Int64.TryParse(mid, out Mid);
            if (!succ) return 0;
            return Mid;

        }


        //Every order will be cancelled after X milliseconds without request
        public bool setAbsenceCancelPolicy(long mseconds)
        {

            var cancelResult = makeReq(REQ.SETABSENCECANCELPOLICY, "" + mseconds);
            if (cancelResult == null) return false;

            if (cancelResult.Contains("success")) return true;
            return false;
        }

        //Warning: sets your current API Account to read only. This cannot be undone!
        public bool setAPIAccountToReadOnly()
        {

            var cancelResult = makeReq(REQ.SETREADONLY, "");
            if (cancelResult == null) return false;

            if (cancelResult.Contains("success")) return true;
            return false;
        }

        public bool setForceNonce(bool forceNonce)
        {

            var cancelResult = makeReq(REQ.SETFORCENONCE, "" + forceNonce);
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
