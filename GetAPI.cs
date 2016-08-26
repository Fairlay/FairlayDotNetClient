using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FairlaySampleClient
{
    //Free Public API for retrieving markets and odds. Check the documentation at:  http://piratepad.net/APVEoUmQPS
           
    class GetAPI
    {
        public Dictionary<long, MarketX> Markets;
        public DateTime LastCheck;
        public long Offset;
        public DateTime LastGetTimeCall;
        private string RequestStr;

        public GetAPI(string requestStr)
        {
            LastCheck = new DateTime(2015, 1, 1);
            LastGetTimeCall = new DateTime(2015, 1, 1);
            Markets = new Dictionary<long, MarketX>();
            Offset = 0;
            RequestStr = requestStr;
        }

        //call this
        public bool grab()
        {

           

            //TODO use server time to calculate offset  http://31.172.83.181:8080/free/time
            if(LastGetTimeCall.AddMinutes(10) < Util1.getUTCNow)
            {
                var time = OpenPage("http://31.172.83.181:8080/free6/time");
                if (time == null) return false;
                Offset = Util1.getUTCNow.Ticks - Convert.ToInt64(time);
                LastGetTimeCall = Util1.getUTCNow;
            }

            var submittedDate = JsonConvert.SerializeObject(LastCheck.AddSeconds(-10).AddTicks(-Offset));
            Random rc = new Random();
            int use = rc.Next(1, 10);
            var lastCheck = Util1.getUTCNow;

            var answer = OpenPage("http://31.172.83.181:8080/free" + use + "/markets/{"+RequestStr+", \"ToID\":10000,\"SoftChangedAfter\":" + submittedDate + "}");


            if (answer == null || answer.Contains("XError")) return false;

           

            var mL = JsonConvert.DeserializeObject<List<MarketX>>(answer);
         

            try
            {
                foreach (var m in mL)
                {
                    if (m.ClosD > Util1.getUTCNow.AddHours(-0.5))
                    {
                        Markets[m.ID] = m;
                    }
                }


                var del = new List<long>();
                foreach (var m in Markets)
                {
                    if (m.Value.ClosD < Util1.getUTCNow.AddHours(-2)) del.Add(m.Key);
                }

                foreach (var mid in del)
                {
                    Markets.Remove(mid);
                }
            }
            catch (Exception ex)
            {
            }

            LastCheck = lastCheck;
          
            return true;
        }

        public static double getOdds(MarketX m, int runner, int boa, int oddsorstake)
        {
            var os = m.OrdBStr;
            if (os == null) return 0d;
            var ruStr = os.Split('~');

            if (ruStr.Length <= runner) return 0d;

            var ruOb = ruStr[runner];

            if (ruOb.Contains("Bids"))
            {

                var ob = JsonConvert.DeserializeObject<OrderBook>(ruOb);
                if (boa == 0 && ob.Bids.Count > 0) return Convert.ToDouble(ob.Bids[0][oddsorstake]);
                else if (boa == 1 && ob.Asks.Count > 0) return Convert.ToDouble(ob.Asks[0][oddsorstake]);
                else return 0d;

            }


            return 0d;
        }


        public static string OpenPage(string url, string Method = WebRequestMethods.Http.Get, byte[] Parameters = null)
        {
            string cont = null;
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
         | SecurityProtocolType.Ssl3;


                var request = CreateRequest(url, Method, Parameters);
                request.Timeout = 5000;
                request.ProtocolVersion = HttpVersion.Version10;
                if (Method == WebRequestMethods.Http.Post)
                {
                    using (var rstream = request.GetRequestStream())
                    {
                        if (Parameters != null)
                            rstream.Write(Parameters, 0, Parameters.Length);
                    }
                }


                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.ContentEncoding.ToUpperInvariant().Contains("GZIP"))
                    {
                        using (var stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                cont = reader.ReadToEnd();
                            }
                        }

                    }
                    else
                    {
                        using (var stream = response.GetResponseStream())
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                cont = reader.ReadToEnd();
                            }
                        }
                    }
                }



            }
            catch (Exception e)
            {
                if (e.ToString().Contains("TLS-Kanal erstellt")) return "SSLError";
              
                return null;
            }

            return cont;
        }

        public static HttpWebRequest CreateRequest(string url, string Method = WebRequestMethods.Http.Get, byte[] Parameters = null)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "Mozilla/5.0 (Windows NT 5.1; rv:25.0) Gecko/20100101 Firefox/25.0";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.AllowAutoRedirect = true;

            request.Method = Method;
            request.Timeout = 25000;



            if (Method == WebRequestMethods.Http.Post && Parameters != null)
            {
                request.ContentLength = Parameters.Length;
                request.ContentType = "application/x-www-form-urlencoded";
            }

            return request;
        }
  
    }
}
