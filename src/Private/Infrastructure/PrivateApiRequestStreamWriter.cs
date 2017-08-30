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

		public async Task WriteRequest(SignedPrivateApiRequest request)
		{
			string apiRequestMessage = request.FormatIntoApiRequestMessage();
			using (var streamWriter = new StreamWriter(requestStream, Encoding.UTF8, 1024, true))
				await streamWriter.WriteAsync(apiRequestMessage);
		}
	}
}