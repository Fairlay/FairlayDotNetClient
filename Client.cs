using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FairlaySampleClient
{

    
    public class Client
    {
        public long Nonce;
        public string PublicKey;
        private string PrivateKey;
        public long ID;
        public long Offset = 0;
        public bool Encrypt;
        public string SymmKey;

        public Client(string privateKey, string publicKey, long id, bool encrypt)
        {
            Nonce = DateTime.Now.Ticks;
            PrivateKey = privateKey;
            PublicKey = publicKey;
            ID = id;
            Encrypt = encrypt;
        }

        public string DoRequestAndVerify(string ip, int port, string str, string PublicKey)
        {
            string answer;

            if (!Encrypt) answer = DoRequest(ip, port, str, 0, null);
            else answer = DoRequest(ip, port, str, 2, SymmKey);

            var reqA = answer.Split('|');
            string signature = reqA[0];
            if (reqA.Length>1 && ( reqA[1].Length == 18 || reqA[1].Length == 19))
            {
                Offset = Util1.getUTCNow.Ticks - Convert.ToInt64(reqA[1]);
            }
            string sigString = answer.Substring(answer.IndexOf("|") + 1);
            var legimate = Util1.VerifyData(sigString, signature, PublicKey);

            if (!legimate) return null;
            if (reqA.Length < 4) return null;

            sigString = sigString.Substring(sigString.IndexOf("|") + 1);
            sigString = sigString.Substring(sigString.IndexOf("|") + 1);

            return sigString;


        }
        public string addNonceAndSign(string req)
        {
            var str = Nonce++ + "|" + ID + "|" + req;
             long newnonce = Util1.getUTCNow.Ticks;
             if (newnonce > Nonce) Nonce = newnonce;
             var sign = Util1.SignData(str, PrivateKey);
           
            return sign + "|" + str;
        }
    
        public string DoRequest(string ip, int port, string req,  int enc, string key)
        {

            var sign = addNonceAndSign(req);
         
            return SendRequest(ip, port, sign,enc, key );


        }

        public static string SendRequest(string ip, int port, string str, int encrypt, string key)
        {
            
            
            string answer = "";
            TcpClient tcpclnt = new TcpClient();

            Random rc = new Random();

            try
            {
                tcpclnt.SendTimeout = 3000;
                tcpclnt.Connect(ip, port);
                Stream stm = tcpclnt.GetStream();
                stm.ReadTimeout = 5000;
                tcpclnt.ReceiveTimeout = 5000;

                UTF8Encoding asen = new UTF8Encoding();

                var str2 = str + "<ENDOFDATA>";
                byte[] ba;

                //if (encrypt == 2 && key != null)
                //{

                //  //  ba = asen.GetBytes(EncryptSymm(str2, key));
                //}
                //else
                {
                    ba = asen.GetBytes(str2);
                }



                if (encrypt == 1 && key != null)
                {
                    //   ba = StringCipher.encrypt(key, ba);
                }
                //  byte[] ba = asen.GetBytes(str );
                stm.Write(ba, 0, ba.Length);




                byte[] bb = new byte[100000];

                int k = 0;
                int k1 = 1;
                while (k1 != 0)
                {
                    k1 = stm.Read(bb, k, 1024);
                    k += k1;

                }

                var bbr = new byte[k];
                for (int i = 0; i < k; i++)
                    bbr[i] = bb[i];

                answer = Util1.Unzip(bbr);




                tcpclnt.Close();
            }


            catch (Exception ex)
            {
                return "";
            }





            return answer;

        }


    }
}
