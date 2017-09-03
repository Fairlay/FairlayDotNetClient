using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using FairlayDotNetClient.Private.Datatypes;
using FairlayDotNetClient.Public;
using Newtonsoft.Json;

namespace FairlayDotNetClient.Private
{
	/// <summary>
	/// For details see https://github.com/Fairlay/FairlayDotNetClient and
	/// https://github.com/Fairlay/FairlayPrivateAPI
	/// </summary>
	public abstract class PrivateApi
	{
		public abstract Task<string> DoApiRequestAndVerify(int requestId, object requestBody = null);
		public abstract int OurUserId { get; }

		public async Task<DateTimeOffset> GetServerTime()
		{
			string response = await DoApiRequestAndVerify(REQ.GETSERVERTIME);
			long serverTimeTicks = long.Parse(response);
			return new DateTimeOffset().AddTicks(serverTimeTicks);
		}

		public async Task<List<MUserTransfer>> GetTransfers(DateTimeOffset sinceDate)
		{
			string response = await DoApiRequestAndVerify(REQ.GETTRANSFERS, sinceDate.UtcTicks);
			return response == null ? null : JsonConvert.DeserializeObject<List<MUserTransfer>>(response);
		}

		/// <summary>
		/// Returns the Balances of all Currencies /0 is Bitcoin /1 Ethereum /2 Litecoin,
		/// Get a full list by querying all available Currencies via GETCURRENCIES
		/// </summary>
		public async Task<Dictionary<int, ReturnBalance>> GetBalances(int currencyID = -1)
		{
			string response = await DoApiRequestAndVerify(REQ.GETMYBALANCEV2, currencyID);
			return response == null ? null
				: JsonConvert.DeserializeObject<Dictionary<int, ReturnBalance>>(response);
		}

		/// <summary>
		/// Transfers funds to another user. By default mBTC (currency ID 0 ) are transfered
		/// </summary>
		public async Task<bool> TransferFunds(int receiverUserId, string description, PaymentType type,
			decimal amount, int currencyId = 0)
		{
			var userTransfer = new MUserTransfer(OurUserId, receiverUserId, description, (int)type,
				amount, currencyId);
			string answer = await DoApiRequestAndVerify(REQ.TRANSFERFUNDS, userTransfer);
			return answer != null;
		}

		public async Task<bool> AddOrChangeCurrency(Currency cur)
		{
			string response = await DoApiRequestAndVerify(REQ.ADDORCHANGECURRENCY, cur);
			return response != null && response.StartsWith("Currency ");
		}

		public async Task<Dictionary<int, Currency>> GetCurrencies()
		{
			string response = await DoApiRequestAndVerify(REQ.GETCURRENCIES);
			return response == null ? null
				: JsonConvert.DeserializeObject<Dictionary<int, Currency>>(response);
		}

		/// <summary>
		/// Check overall reserves: https://blockchain.info/address/1EVrCZ3ahUucq3jvAqTTPTZe1tySniXuWi
		/// </summary>
		public async Task<bool> VerifyProofOfReserves(decimal userBalance, string mustContain,
			string publicTopHash, decimal sumFunds)
		{
			string thash = await DoApiRequestAndVerify(REQ.GETTOPHASH);
			if (string.IsNullOrEmpty(thash)) return false;
			var tophash = JsonConvert.DeserializeObject<TopHash>(thash);
			if (tophash == null || (publicTopHash != null && tophash.Hash != publicTopHash)) return false;
			string myid = await DoApiRequestAndVerify(REQ.GETMYPROOFID);
			if (myid == null) return false;
			if (mustContain != null && !myid.Contains(mustContain)) return false;
			string proof = await DoApiRequestAndVerify(REQ.GETPROOFOFRESERVES);
			if (proof == null) return false;
			bool verified = ProofUser.VerifyUserBranches(
				JsonConvert.DeserializeObject<ProofBlindBranch[]>(proof), myid, userBalance, tophash.Hash,
				sumFunds);
			return verified;
		}

		public async Task<string[]> GetOrderbook(long mid)
		{
			string ts = await DoApiRequestAndVerify(REQ.GETORDERBOOK, mid);
			return ts?.Split('~');
		}

		public async Task<List<MatchedOrder>> GetLatestbets5Min()
		{
			string response = await DoApiRequestAndVerify(REQ.LATESTBETS5MIN);
			return response == null ? null : JsonConvert.DeserializeObject<List<MatchedOrder>>(response);
		}

		public async Task<List<MatchedOrder>> GetLatestbets60Min()
		{
			string response = await DoApiRequestAndVerify(REQ.LATESTBETS60MIN);
			return response == null ? null : JsonConvert.DeserializeObject<List<MatchedOrder>>(response);
		}

		/// <summary>
		/// Retrieve all settlements from the last 60 minutes.
		/// </summary>
		public async Task<List<SettleReq>> GetLatestSettlements()
		{
			string response = await DoApiRequestAndVerify(REQ.LATESTSETTLEMENTS1H);
			return response == null ? null : JsonConvert.DeserializeObject<List<SettleReq>>(response);
		}

		// Provide the UTC time in Ticks.
		public async Task<List<MUserStatement>> GetStatement(DateTime sinceDate)
		{
			string answer = await DoApiRequestAndVerify(REQ.GETSTATEMENT, sinceDate.Ticks);
			return answer == null ? null : JsonConvert.DeserializeObject<List<MUserStatement>>(answer);
		}

		/// <summary>
		/// Provide Dates in UTC
		/// </summary>
		public async Task<List<ReturnUOrder>> GetLongUOrderList(long time)
		{
			int start = 0;
			int maxitems = 1500;
			var allRet = new Dictionary<long, ReturnUOrder>();
			while (true)
			{
				int fromID = Math.Max(0, start - 10);
				int toID = start + maxitems;
				//var sfdx = placeOrder(3017402, 0, 0, 1.01m, 10, "");
				//var x1 = await DoApiRequestAndVerify(REQ.CHANGECLOSINGTIME, 3017402 + "|" + DateTime.UtcNow.AddHours(1));
				string ret = await DoApiRequestAndVerify(REQ.GETUNMATCHEDORDERS,
					time + "|" + fromID + "|" + toID);
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
						if (!allRet.ContainsKey(m._UserOrder.OrderID))
							allRet.Add(m._UserOrder.OrderID, m);
						else
							allRet[m._UserOrder.OrderID] = m;
					}
					if (getm.Count < maxitems) break;
					start += maxitems;
				}
				catch (Exception) { break; }
			}
			return new List<ReturnUOrder>(allRet.Values);
		}

		public async Task<List<ReturnMOrder>> GetLongMOrderList(long time)
		{
			int start = 0;
			int maxitems = 1500;
			var allRet = new Dictionary<long, ReturnMOrder>();
			while (true)
			{
				int fromID = Math.Max(0, start - 10);
				int toID = start + maxitems;
				string ret = await DoApiRequestAndVerify(REQ.GETMATCHEDORDERS,
					time + "|" + fromID + "|" + toID);
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
						if (!allRet.ContainsKey(m._UserOrder.OrderID))
							allRet.Add(m._UserOrder.OrderID, m);
						else
							allRet[m._UserOrder.OrderID] = m;
					}
					if (getm.Count < maxitems) break;
					start += maxitems;
				}
				catch (Exception) { break; }
			}
			return new List<ReturnMOrder>(allRet.Values);
		}

		public async Task<List<ReturnMOrder>> GetMOrderList(long time, long mid = 0)
		{
			string ret = await DoApiRequestAndVerify(REQ.GETMATCHEDORDERS,
				time + (mid != 0 ? "|" + mid : ""));
			return JsonConvert.DeserializeObject<List<ReturnMOrder>>(ret);
		}

		/// <summary>
		/// Allows you to create, cancel and alter orders: Set Pri to 0  to cancel an order, Set Oid to
		/// -1 to create an order. Maximum allowed orders in one request: 50
		/// </summary>
		public async Task<bool> ChangeOrders(REQChangeOrder[] coL)
		{
			if (coL.Length > 50) return false;
			var answL = await SendChangeOrders(coL);
			if (answL == null)
				return false;
			var marketsToCancel = new List<long>();
			for (int i = 0; i < answL.Length; i++)
			{
				var co = answL[i];
				string answ = co.Res;
				if (answ.Contains("YError:Market Closed") || answ == "Order cancelled")
				{
					//order cancelled
				}
				else if (answ.Contains("YError"))
					marketsToCancel.Add(co.Mid);
				else
				{
					try
					{
						JsonConvert.DeserializeObject<UnmatchedOrder>(answ);
						//order successfully created or changed
					}
					catch (Exception)
					{
						return false;
					}
				}
			}
			await CancelOrdersOnMarket(marketsToCancel);
			return true;
		}

		public async Task<REQChangeOrder[]> SendChangeOrders(REQChangeOrder[] coL)
		{
			string ret = await DoApiRequestAndVerify(REQ.CHANGEORDERSV2, coL);
			return ret == null ? null : JsonConvert.DeserializeObject<REQChangeOrder[]>(ret);
		}

		/// <summary>
		/// State a reason for the cancellation if possible, which is forwarded to the other party
		/// 0:  not provided
		/// 1:  other reason 
		/// 10: line changed
		/// 11: market offline
		/// 12: market  closed
		/// orderID represents the ID of the MATCHED order.
		/// </summary>
		public async Task<bool> MakervoidMatchedOrder(long mid,
			int ruid, long orderID, int reason)
		{
			string cancelResult = await DoApiRequestAndVerify(REQ.CANCELMATCHEDORDER,
				mid + "|" + reason + "|" + +ruid + "|" + orderID);
			return cancelResult != null && IsValidOrderResult(cancelResult);
		}

		private static bool IsValidOrderResult(string cancelResult)
			=> cancelResult.Contains("Cancellation successful") ||
				cancelResult.Contains("Order will be cancelled after Activation") ||
				cancelResult.Contains("Order does not exist");

		/// <summary>
		/// Reason can be set to 0 if the order is confirmed, please call this when possible for faster
		/// confirmation for the other party. orderID represents the ID of the MATCHED order. set the
		/// am(ount) to -1  to fully confirm tbe order, if only a part of it shall be confirmed, define
		/// the amount accordingly.
		/// </summary>
		public async Task<bool> ConfirmMatchedOrder(long mid,
			int ruid, long orderID, int reason, decimal am = -1m)
		{
			string cancelResult = await DoApiRequestAndVerify(REQ.CONFIRMMATCHEDORDER,
				mid + "|" + reason + "|" + ruid + "|" + orderID + "|" +
				am.ToString(CultureInfo.InvariantCulture));
			return cancelResult != null && cancelResult.Contains("Confirmation successful");
		}

		public async Task<bool> CancelAllOrders()
		{
			string resp = await DoApiRequestAndVerify(REQ.CANCELALLORDERS);
			return resp != null && resp.Contains("cancelled");
		}

		public async Task<bool> CancelOrdersOnMarket(List<long> mids)
		{
			if (mids == null || mids.Count == 0) return true;
			string resp = await DoApiRequestAndVerify(REQ.CANCELORDERSONMARKETS, mids);
			return resp != null && resp.Contains("cancelled");
		}

		/// <summary>
		/// See the LMSR object for documentation to disable a MM, set Enabled to false;
		/// </summary>
		public async Task<LMSR> SetLmsr(LMSR lmsr)
		{
			if (lmsr == null) return null;
			string answer = await DoApiRequestAndVerify(REQ.SETMARKETMAKER1, lmsr);
			if (answer == null || answer.StartsWith("XError")) return null;
			return JsonConvert.DeserializeObject<LMSR>(answer);
		}

		public async Task<List<LMSR>> GetAllLmsRs()
		{
			string answer = await DoApiRequestAndVerify(REQ.GETMARKETMAKER, "-1");
			if (answer == null || answer.StartsWith("XError")) return null;
			return JsonConvert.DeserializeObject<List<LMSR>>(answer);
		}

		public async Task<long> CreateMarket(MarketX m)
			=> long.TryParse(await DoApiRequestAndVerify(REQ.CREATEMARKET, m), out long mid)
				? mid : 0;

		public async Task<bool> ChangeMarketTimes(long mid,
			DateTime closD, DateTime setlD)
			=> await DoApiRequestAndVerify(REQ.CHANGETIMEREQUEST, new ChangeTimeReq
			{
				MID = mid,
				ClosD = closD,
				SetlD = setlD
			}) != null;

		/// <summary>
		/// Settle a market you created. The full turnover of the market will be deducted from your
		/// Available Balance for 3 to 4 days.
		/// </summary>
		public async Task<bool> SettleMarket(long mid, int runnerid,
			bool win = true, bool half = false)
		{
			string response = await DoApiRequestAndVerify(REQ.SETTLEREQUEST, new SettleReq
			{
				Mid = mid,
				Runner = runnerid,
				Win = win ? 1 : 2,
				Half = half
			});
			return response != null && response.Contains("settled");
		}

		/// <summary>
		/// To delete an existing API Account, set PublicRSAKeyXML to "DELETE"  and send the request
		/// from the native API Account #0. The transferLimit should only be set for TransferOnly API
		/// Accounts. If the transferLimit is set to 0, it means that there is no limit.
		/// </summary>
		public async Task<bool> RegisterApiAccount(int apiAccount,
			string publicRSAKeyXml, bool isReadOnly, bool isTransferOnly,
			decimal dailyTransferLimitmBtc = 0)
		{
			string req = string.Format("{0}|{1}|{2}|{3}|{4}", apiAccount, publicRSAKeyXml, isReadOnly,
				isTransferOnly, dailyTransferLimitmBtc.ToString(CultureInfo.InvariantCulture));
			string response = await DoApiRequestAndVerify(REQ.REGISTERAPI2, req);
			return response == "New APIUser Added";
		}

		public async Task<bool> SetPublicUserName(string name)
		{
			string answer = await DoApiRequestAndVerify(REQ.SETSCREENNAME, name);
			return answer != null && answer == "success";
		}

		/// <summary>
		/// If you are the maker of a bet, you will be notified via a TCP request with all details about
		/// the matched order make sure you listen on the selected port and ip and that your firewall is
		/// not blocking the requests each callback costs 1 request the callbacks will automatically
		/// stop if they fail more than 100 times or if you do not have enough requests left. Special
		/// Permission dependent on the node you are connecting to are currently required make sure the
		/// runner market you are betting on allows you to cancel (makervoid) bets after they are
		/// matched  (VisDelay must be >0) also, you must set makerCT of your UnmatchedOrder to a value
		/// less or equal to the VisDelay of the Runner Market you are betting on you are than able to
		/// void any MatchedOrder within "makerCT" milliseconds. After that the MatchedOrder will go
		/// from State PENDING to MATCHED.
		/// </summary>
		public async Task<bool> SetCallbackIP(string ip, int port)
		{
			string answer = await DoApiRequestAndVerify(REQ.SETCALLBACKIP, ip + ":" + port);
			return answer != null && answer == "success";
		}

		/// <summary>
		/// You will receive mBTCs in exchange for fairlay credits if you send funds to the user 111111
		/// for example. withdrawal fees are subject to change.
		/// </summary>
		public Task<bool> DoWithdrawal(string address, decimal amountMbtc)
			=> TransferFunds(111111, address, PaymentType.Payments, amountMbtc);

		/// <summary>
		/// Every unmatched order of your account will be cancelled after X milliseconds without any
		/// request of any API Account.
		/// </summary>
		public async Task<bool> SetAbsenceCancelPolicy(long mseconds)
		{
			string cancelResult = await DoApiRequestAndVerify(REQ.SETABSENCECANCELPOLICY, mseconds);
			return cancelResult != null && cancelResult.Contains("success");
		}

		/// <summary>
		/// Warning: sets your current API Account to read only. This cannot be undone! Never set your
		/// native api account #0 to read-only as you won't be able to create another API Account
		/// </summary>
		public async Task<bool> SetApiAccountToReadOnly()
		{
			string cancelResult = await DoApiRequestAndVerify(REQ.SETREADONLY);
			return cancelResult != null && cancelResult.Contains("success");
		}

		/// <summary>
		/// Force Nonce with every Requests - All GET-requests do not require a nonce by default
		/// </summary>
		public async Task<bool> SetForceNonce(bool forceNonce)
		{
			string cancelResult = await DoApiRequestAndVerify(REQ.SETFORCENONCE, forceNonce);
			return cancelResult != null && cancelResult.Contains("success");
		}

		/// <summary>
		/// For market makers. If you set ForceConfirm to false (default) all bets that are matched
		/// against your existing orders will go into the state MATCHED after the MAKERCANCELTIME has
		/// passed and no confirmation or cancellation was sent by you. If set to true, your orders will
		/// go into the state MAKERVOIDED after the MAKERCANCELTIME has passed and no confirmation or
		/// cancellation was sent by you.
		/// </summary>
		public async Task<bool> SetForceConfirm(bool force)
		{
			string cancelResult = await DoApiRequestAndVerify(REQ.SETFORCECONFIRM, force);
			return cancelResult != null && cancelResult.Contains("success");
		}

		public async Task<bool> SetSettleDelegates(long[] accountIds)
		{
			string cancelResult = await DoApiRequestAndVerify(REQ.SETSETTLEDELEGATES, accountIds);
			return cancelResult != null && cancelResult.Contains("success");
		}
	}
}