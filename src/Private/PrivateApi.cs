using System.Threading.Tasks;

namespace FairlayDotNetClient.Private
{
	public interface PrivateApi
	{
		Task<string> DoApiRequestAndVerify(string requestHeader, string requestBody = null);
	}
}