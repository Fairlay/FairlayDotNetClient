using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairlaySampleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            TestClient tc = new TestClient();
            
            Console.WriteLine("Your private and public key were saved in the config.txt file:\r\n" + tc.getPublicKey());
            bool suc = tc.init(0);



            var _GetAPI = new GetAPI("\"Cat\":12 ,\"TypeOr\":[0],\"PeriodOr\":[1], \"OnlyActive\":true");

            // call grab every 10 seconds to update your markets.
            bool suc_grab = _GetAPI.grab();
            if(suc_grab)
            {

                //use  _GetAPI.Markets;
            }

            if(!suc)
            { 
                Console.WriteLine("\r\nPlease Enter your UserID");
                string line = Console.ReadLine(); 
                int id;
                bool valid = Int32.TryParse(line, out id);
                if(!valid) return;
                suc= tc.init(id);
            }
            var nextPresidentOrderbook2016 = tc.getOrderbook(72633292476);

            if(suc)
            {
                Console.WriteLine("\r\nConnected!");

            }
            else return;

            var xf = tc.getBalance();
            Console.WriteLine("\r\nYour balance is: " + JsonConvert.SerializeObject(xf));

            bool verifiedProof = tc.VerifyProofOfReserves(xf.PrivReservedFunds);
            while(true)
            {
                Console.WriteLine("\r\nEnter Command");
                var read = Console.ReadLine();
            }
        }

        
    }
}
