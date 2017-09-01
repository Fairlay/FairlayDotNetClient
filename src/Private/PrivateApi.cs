using System.Threading.Tasks;

namespace FairlayDotNetClient.Private
{
	public interface PrivateApi
	{
		Task<string> DoRequestAndVerify(string requestHeader, string requestBody = null);
	}
}