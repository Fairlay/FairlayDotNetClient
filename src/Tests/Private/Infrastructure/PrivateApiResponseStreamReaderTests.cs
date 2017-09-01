using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using FairlayDotNetClient.Private.Infrastructure;
using NUnit.Framework;

namespace FairlayDotNetClient.Tests.Private.Infrastructure
{
	public class PrivateApiResponseStreamReaderTests
	{
		[Test]
		public async Task ReadResponseFromCompressedStream()
		{
			using (var responseStream = new MemoryStream())
			using (var zipStream = new GZipStream(responseStream, CompressionMode.Compress))
			{
				var responseReader = new PrivateApiResponseStreamReader(responseStream);
				string apiResponseMessage = TestData.ApiResponse.FormatIntoApiResponseMessage();
				var apiReponseMessageData = Encoding.UTF8.GetBytes(apiResponseMessage);
				zipStream.Write(apiReponseMessageData, 0, apiReponseMessageData.Length);
				zipStream.Flush();
				responseStream.Seek(0, SeekOrigin.Begin);
				var parsedResponse = await responseReader.ReadResponse();
				parsedResponse.AssertIsValueEquals(TestData.ApiResponse);
			}
		}
	}
}