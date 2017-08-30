using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using FairlayDotNetClient.Private.Responses;

namespace FairlayDotNetClient.Private.Infrastructure
{
	public class PrivateApiResponseStreamReader
	{
		public PrivateApiResponseStreamReader(Stream responseStream) => this.responseStream = responseStream;

		private readonly Stream responseStream;

		public async Task<PrivateApiResponse> ReadResponse()
		{
			using (var zipStream = new GZipStream(responseStream, CompressionMode.Decompress, true))
			using (var streamReader = new StreamReader(zipStream, Encoding.UTF8))
			{
				string apiResponse = await streamReader.ReadToEndAsync();
				return PrivateApiResponseExtensions.CreateFromApiResponseMessage(apiResponse);
			}
		}
	}
}