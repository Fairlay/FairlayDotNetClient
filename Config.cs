using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace FairlaySampleClient
{

    public class ConfigDet
    {

        public string PrivateRSAKey;
        public string PublicRSAKey;
      
        public int ID;
        public string SERVERIP;
        public int PORT;
        public int APIAccountID;

        public ConfigDet(bool generate)
        {
            if (generate)
            {
                PrivateRSAKey = Util1.generatePrivKey();
                PublicRSAKey = Util1.getPubKey(PrivateRSAKey);
            }
        }
        public void WriteToFile(string path = "config.txt")
        {
            try
            {

                StreamWriter sr = new StreamWriter(path);
                sr.Write(JsonConvert.SerializeObject(this));
                sr.Close();


            }
            catch (Exception ex)
            {
            

            }
        }

        public static ConfigDet ReadConfig(string path = "config.txt")
        {


            try
            {


                using (System.IO.StreamReader reader = new System.IO.StreamReader(path, System.Text.Encoding.Default))  //openDialog.FileName
                {
                    string line;
                    int j = 0;
                    line=reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<ConfigDet>(line);

                }

            }
            catch (Exception ex)
            {
               
            }

            return null;
        }

    }

}
