using System.Net;
using FairlayDotNetClient.Private.Infrastructure;

namespace FairlayDotNetClient.Tests.Private.Infrastructure
{
	public class MockPrivateApiConnection : StreamingPrivateApiConnection
	{
		public override void SetEndpoint(IPEndPoint endpoint) { }
	}
}