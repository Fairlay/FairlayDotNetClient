using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FairlayDotNetClient.Public
{
	//ncrunch: no coverage start
	public class FairlayPublicApi : PublicApi
	{
		protected override Task<string> GetHttpResponse(string method, string parameters)
			=> GetHttpResponse("http://31.172.83.181:8080/free" + new Random().Next(1, 10) + "/" +
				method + "/"+parameters);

		private static async Task<string> GetHttpResponse(string url)
			=> await (await new HttpClient(AutoDecompression).GetAsync(url)).Content.ReadAsStringAsync();

		public static HttpMessageHandler AutoDecompression
			=> new HttpClientHandler
			{
				AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
			};
	}
}