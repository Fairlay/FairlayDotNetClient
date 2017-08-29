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

		public Task Write(SignedPrivateApiRequest request)
		{
			string serverMessage = request.FormatIntoServerMessage();
			var serverMessageData = Encoding.UTF8.GetBytes(serverMessage);
			return requestStream.WriteAsync(serverMessageData, 0, serverMessageData.Length);
		}
	}
}