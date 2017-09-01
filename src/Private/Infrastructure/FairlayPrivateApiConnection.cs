using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using FairlayDotNetClient.Private.Requests;
using FairlayDotNetClient.Private.Responses;

namespace FairlayDotNetClient.Private.Infrastructure
{
	public class FairlayPrivateApiConnection : BasePrivateApiConnection
	{
		public override void SetEndpoint(IPEndPoint endPoint) => currentEndpoint = endPoint;

		private IPEndPoint currentEndpoint;

		public override async Task<PrivateApiResponse> DoApiRequest(SignedPrivateApiRequest request)
		{
			var socket = CreateSocketFromCurrentEndPoint();
			await socket.ConnectAsync(currentEndpoint.Address, currentEndpoint.Port);
			using (var networkStream = new NetworkStream(socket, true))
				return await SetBidirectionalStreamAndDoRequest(request, networkStream);
		}

		private static Socket CreateSocketFromCurrentEndPoint()
			=> new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
				{
					NoDelay = true,
					LingerState = new LingerOption(true, SockerLingerTimeoutInSeconds)
				};

		private const int SockerLingerTimeoutInSeconds = 2;

		private Task<PrivateApiResponse> SetBidirectionalStreamAndDoRequest(
			SignedPrivateApiRequest request, Stream bidirectionalStream)
		{
			SetRequestStream(bidirectionalStream);
			SetResponseStream(bidirectionalStream);
			return base.DoApiRequest(request);
		}
	}
}