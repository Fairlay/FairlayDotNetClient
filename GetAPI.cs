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

        //call this
        public bool grab(string requestStr)
        {

            if (Markets == null)
            {
                LastCheck = new DateTime(2015, 1, 1);
                Markets = new Dictionary<long, MarketX>();
            }
            var submittedDate = JsonConvert.SerializeObject(LastCheck.AddMinutes(-10));
            Random rc = new Random();
            int use = rc.Next(1, 9);

            var answer = OpenPage("http://31.172.83.181:8080/free" + use + "/markets/{"+requestStr+", \"ToID\":10000,\"SoftChangedAfter\":" + submittedDate + "}", 0);


            LastCheck = Util1.getUTCNow;
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


        public static string OpenPage(string url, int tryx, string Method = WebRequestMethods.Http.Get, byte[] Parameters = null)
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
