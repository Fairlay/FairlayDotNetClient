using System.Net;
using System.Security.Cryptography;

namespace FairlayDotNetClient.Private
{
	public class PrivateApiCredentials
	{
		public long UserId { get; set; }
		public int ApiAccountId { get; set; }
		public RSAParameters PrivateKey { get; set; }
		public IPEndPoint Server { get; set; }
	}
}