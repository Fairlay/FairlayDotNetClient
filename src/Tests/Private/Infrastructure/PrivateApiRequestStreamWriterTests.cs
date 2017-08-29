using System.IO;
using System.Text;
using System.Threading.Tasks;
using FairlayDotNetClient.Private.Infrastructure;
using NUnit.Framework;

namespace FairlayDotNetClient.Tests.Private.Infrastructure
{
	public class PrivateApiRequestStreamWriterTests
	{
		[Test]
		public async Task TestWriteRequest()
		{
			using (var requestStream = new MemoryStream())
			using (var streamReader = new StreamReader(requestStream, Encoding.UTF8))
			{
				var requestWriter = new PrivateApiRequestStreamWriter(requestStream);
				var request = TestData.SignedRequest;
				await requestWriter.Write(request);
				requestStream.Seek(0, SeekOrigin.Begin);
				string writtenRequest = streamReader.ReadToEnd();
				Assert.That(writtenRequest, Is.EqualTo(request.FormatIntoServerMessage()));
			}
		}
	}
}