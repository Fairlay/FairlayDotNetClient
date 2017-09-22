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
			=> ServerEndPoint = new IPEndPoint(IPAddress.Parse("31.172.83.53"), 18017);

		public int UserId { get; set; }
		public int ApiAccountId { get; set; }
		public RSAParameters PrivateRsaParameters { get; set; }
		public IPEndPoint ServerEndPoint { get; set; }
	}
}