using System.Collections.Generic;

namespace FairlayDotNetClient.Public
{
	public class OrderBook
	{
		public List<decimal[]> Bids;
		public List<decimal[]> Asks;
		public int S = 0;
	}
}