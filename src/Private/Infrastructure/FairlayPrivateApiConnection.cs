using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using FairlayDotNetClient.Private.Requests;
using FairlayDotNetClient.Private.Responses;
using System.Threading;

namespace FairlayDotNetClient.Private.Infrastructure
{
	public class FairlayPrivateApiConnection : BasePrivateApiConnection
	{
		public override void SetEndPoints(List<IPEndPoint> setEndPoints) => endPoints = setEndPoints;
		private List<IPEndPoint> endPoints;

		public override async Task<PrivateApiResponse> DoApiRequest(SignedPrivateApiRequest request)
		{
			// Only one usage of each socket address (protocol/network address/port) is normally permitted
			// 31.172.83.53:18017 on Fairlay Wallet.Service/SocialBot.Service/Exchange.Service
			await preventMultipleCallsToSameAddress.WaitAsync();
			try
			{
				return await ConnectAndDoRequest(endPoints[0], request);
			}
			catch
			{
				try
				{
					// Try next one in list and if it works move first broken server to end of the list
					var result = await ConnectAndDoRequest(endPoints[1], request);
					var firstNotWorking = endPoints[0];
					endPoints.Remove(endPoints[0]);
					endPoints.Add(firstNotWorking);
					return result;
				}
				catch
				{
					// If that didn't help go to the current end of the list (usually the next as we have 3)
					// and do the same, move the currently broken first to the end and use the last next time.
					// If this fails too we are outa here and will crash with the exception thrown here.
					var result = await ConnectAndDoRequest(endPoints.Last(), request);
					var lastWorking = endPoints.Last();
					endPoints.Remove(lastWorking);
					endPoints.Insert(0, lastWorking);
					return result;
				}
			}
			finally
			{
				preventMultipleCallsToSameAddress.Release();
			}
		}

		private async Task<PrivateApiResponse> ConnectAndDoRequest(IPEndPoint useEndPoint,
			SignedPrivateApiRequest request)
		{
			var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
			{
				NoDelay = true,
				LingerState = new LingerOption(true, SockerLingerTimeoutInSeconds)
			};
			await socket.ConnectAsync(useEndPoint.Address, useEndPoint.Port);
			using (var networkStream = new NetworkStream(socket, true))
				return await SetBidirectionalStreamAndDoRequest(request, networkStream);
		}

		private static readonly SemaphoreSlim preventMultipleCallsToSameAddress = new SemaphoreSlim(1);

		public object PreventUsageOfSamePort { get; set; }

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