using System;

namespace FairlayDotNetClient.Tests
{
	/// <summary>
	/// NCrunch allows to easily and quickly test everything with every keystroke: http://ncrunch.net
	/// </summary>
	public static class NCrunchExtensions
	{
		public static bool StartedFromNCrunch => Environment.GetEnvironmentVariable("NCrunch") == "1";
	}
}