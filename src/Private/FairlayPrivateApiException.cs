using System;

namespace FairlayDotNetClient.Private
{
	public class FairlayPrivateApiException : Exception
	{
		public FairlayPrivateApiException(string error) : base(error) {}
	}
}