using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;

namespace FairlayDotNetClient.Private
{
	public class PrivateApiCredentials
	{
		public PrivateApiCredentials(int userId, int apiAccountId = 1) : this()
		{
			UserId = userId;
			ApiAccountId = apiAccountId;
		}

		public PrivateApiCredentials()
			=> ServerEndPoints = new List<IPEndPoint>
			{
				new IPEndPoint(IPAddress.Parse("31.172.83.53"), 18017),
				//currently offline: new IPEndPoint(IPAddress.Parse("31.172.83.66"), 18017),
				new IPEndPoint(IPAddress.Parse("31.172.83.181"), 18017)
			};

		public int UserId { get; set; }
		public int ApiAccountId { get; set; }
		public RSAParameters PrivateRsaParameters { get; set; }
		/// <summary>
		/// Keep a list of possible fairlay server end points, whenever one stops responding we will
		/// put it at the end of the list and use the next one on top instead.
		/// </summary>
		public List<IPEndPoint> ServerEndPoints { get; }
	}
}