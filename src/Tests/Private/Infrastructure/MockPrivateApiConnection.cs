using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FairlayDotNetClient.Private.Infrastructure;
using FairlayDotNetClient.Private.Requests;
using FairlayDotNetClient.Private.Responses;

namespace FairlayDotNetClient.Tests.Private.Infrastructure
{
	public class MockPrivateApiConnection : BasePrivateApiConnection
	{
		public MockPrivateApiConnection() => SetRequestStream(Stream.Null);

		public void SetFakeResponse(PrivateApiResponse fakeResponse) => currentFakeResponse = fakeResponse;

		private PrivateApiResponse currentFakeResponse;

		public override Task<PrivateApiResponse> DoApiRequest(SignedPrivateApiRequest request)
		{
			WriteFakeApiResponseToInMemoryResponseStream();
			return base.DoApiRequest(request);
		}

		/// <summary>
		/// Emulates how the real private API server is encoding a response message so the base 
		/// <see cref="DoApiRequest"/> implementation can decode it properly again.
		/// </summary>
		private void WriteFakeApiResponseToInMemoryResponseStream()
		{
			var inMemoryResponseStream = new MemoryStream();
			SetResponseStream(inMemoryResponseStream);
			string apiResponseMessage = currentFakeResponse.FormatIntoApiResponseMessage();
			var apiResponseMessageData = Encoding.UTF8.GetBytes(apiResponseMessage);
			using (var zipStream = new GZipStream(inMemoryResponseStream, CompressionMode.Compress, true))
				zipStream.Write(apiResponseMessageData, 0, apiResponseMessageData.Length);
			inMemoryResponseStream.Seek(0, SeekOrigin.Begin);
		}

		public override void SetEndpoint(IPEndPoint endPoint) { } //ncrunch: no coverage
	}
}