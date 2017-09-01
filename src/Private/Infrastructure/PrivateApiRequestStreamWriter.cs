using System.IO;
using System.Text;
using System.Threading.Tasks;
using FairlayDotNetClient.Private.Requests;

namespace FairlayDotNetClient.Private.Infrastructure
{
	public class PrivateApiRequestStreamWriter
	{
		public PrivateApiRequestStreamWriter(Stream requestStream) => this.requestStream = requestStream;

		private readonly Stream requestStream;

		public Task WriteRequest(SignedPrivateApiRequest request)
		{
			string apiRequestMessage = request.FormatIntoApiRequestMessage();
			var apiRequestMessageData = Encoding.UTF8.GetBytes(apiRequestMessage);
			return requestStream.WriteAsync(apiRequestMessageData, 0, apiRequestMessageData.Length);
		}
	}
}