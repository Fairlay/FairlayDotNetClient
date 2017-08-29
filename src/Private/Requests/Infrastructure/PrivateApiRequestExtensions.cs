namespace FairlayDotNetClient.Private.Requests.Infrastructure
{
	public static class PrivateApiRequestExtensions
	{
		/// <summary>
		/// https://github.com/Fairlay/FairlayDotNetClient/wiki/Private-API#fairlay-private-api-documentation-v0
		/// </summary>
		public static string FormatIntoSignableString(this PrivateApiRequest request, long nonce)
			=> $"{nonce}|{request.UserId}|{request.Header}|{request.Body}";
	}
}