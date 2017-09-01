using System.Net;
using System.Security.Cryptography;

namespace FairlayDotNetClient.Private
{
	public class PrivateApiCredentials
	{
		public long UserId { get; set; }
		public int ApiAccountId { get; set; }
		public RSAParameters PrivateRsaParameters { get; set; }
		public IPEndPoint ServerEndPoint { get; set; }
	}
}