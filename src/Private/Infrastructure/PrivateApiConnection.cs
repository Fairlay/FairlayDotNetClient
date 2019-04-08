using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FairlayDotNetClient.Private.Requests;
using FairlayDotNetClient.Private.Responses;

namespace FairlayDotNetClient.Private.Infrastructure
{
	public interface PrivateApiConnection
	{
		void SetEndPoints(List<IPEndPoint> setEndPoints);
		Task<PrivateApiResponse> DoApiRequest(SignedPrivateApiRequest request);
	}
}