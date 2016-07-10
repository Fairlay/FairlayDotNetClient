using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairlaySampleClient
{
    
    public class OrderBook
    {
        public List<decimal[]> Bids;
        public List<decimal[]> Asks;
        public int S = 0;

    }
    public partial class Runner 
    {
       
       
        public string Name { get; set; }

        public int VisDelay { get; set; }


        public Runner(string name,  int visibleDelay)
        {
           
            VisDelay = visibleDelay;

            Name = name;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Name + " ");
           
            return sb.ToString();
        }







       
    }
}
