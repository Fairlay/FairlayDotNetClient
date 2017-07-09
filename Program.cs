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

            var balances = tc.getBalance();
            Console.WriteLine("\r\nYour balances are: " + JsonConvert.SerializeObject(balances));
            

            //Do a proof of reserve

            //provide your email or custom username
            string yourusernameoremail = null;

            //retrieve from official source
            string publictophash = null;

            //retrieve from https://blockchain.info/address/1EVrCZ3ahUucq3jvAqTTPTZe1tySniXuWi
            decimal sumFunds = 1700240m;


            if(yourusernameoremail !=null)  tc.setPublicUserName(yourusernameoremail);
           
            bool verifiedProof = tc.VerifyProofOfReserves(balances[0].PrivReservedFunds, yourusernameoremail, publictophash, sumFunds);

            Console.WriteLine("\r\nIs your balance verified?  " + verifiedProof);
        
            while(true)
            {
                Console.WriteLine("\r\nEnter Command");
                var read = Console.ReadLine();
            }
        }

        
    }
}
