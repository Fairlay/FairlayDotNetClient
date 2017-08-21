namespace FairlayDotNetClient.PrivateApi
{
	public class PrivateApiResponseStatus
	{
		public bool Succeeded { get; set; }
		public PrivateApiErrorType ErrorType { get; set; }
		public string ErrorMessage { get; set; }
	}
}