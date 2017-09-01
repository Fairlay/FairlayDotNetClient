using System.IO;
using System.Net;
using System.Threading.Tasks;
using FairlayDotNetClient.Private.Requests;
using FairlayDotNetClient.Private.Responses;

namespace FairlayDotNetClient.Private.Infrastructure
{
	public abstract class BasePrivateApiConnection : PrivateApiConnection
	{
		protected void SetRequestStream(Stream requestStream)
			=> requestWriter = new PrivateApiRequestStreamWriter(requestStream);

		private PrivateApiRequestStreamWriter requestWriter;

		protected void SetResponseStream(Stream responseStream) =>
			responseReader = new PrivateApiResponseStreamReader(responseStream);

		private PrivateApiResponseStreamReader responseReader;

		public virtual async Task<PrivateApiResponse> DoApiRequest(SignedPrivateApiRequest request)
		{
			await requestWriter.WriteRequest(request);
			return await responseReader.ReadResponse();
		}

		public abstract void SetEndpoint(IPEndPoint endPoint);
	}
}