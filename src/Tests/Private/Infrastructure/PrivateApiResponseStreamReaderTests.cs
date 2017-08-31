using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using FairlayDotNetClient.Private.Infrastructure;
using FairlayDotNetClient.Private.Responses;
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
			using (var streamWriter = new StreamWriter(zipStream, Encoding.UTF8))
			{
				var responseReader = new PrivateApiResponseStreamReader(responseStream);
				streamWriter.Write(TestData.ApiResponse.FormatIntoApiResponseMessage());
				FlushTextWriterAndResetStreamPosition(streamWriter, responseStream);
				var parsedResponse = await responseReader.ReadResponse();
				parsedResponse.AssertIsValueEquals(TestData.ApiResponse);
			}
		}

		private static void FlushTextWriterAndResetStreamPosition(TextWriter streamWriter,
			Stream responseStream)
		{
			streamWriter.Flush();
			responseStream.Seek(0, SeekOrigin.Begin);
		}
	}
}