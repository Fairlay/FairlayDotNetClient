using System.IO;
using System.Net;
using System.Threading.Tasks;
using FairlayDotNetClient.Private.Requests;
using FairlayDotNetClient.Private.Responses;

namespace FairlayDotNetClient.Private.Infrastructure
{
	public abstract class StreamingPrivateApiConnection : PrivateApiConnection
	{
		protected void SetConnectionStream(Stream connectionStream) =>
			currentConnectionStream = connectionStream;

		private Stream currentConnectionStream;

		public virtual Task<PrivateApiResponse> DoRequest(SignedPrivateApiRequest request) => null;

		public abstract void SetEndpoint(IPEndPoint endpoint);
	}
}