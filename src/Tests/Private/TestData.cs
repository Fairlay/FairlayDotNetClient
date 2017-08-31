using System;
using System.Net;
using System.Security.Cryptography;
using FairlayDotNetClient.Private;
using FairlayDotNetClient.Private.Requests;
using FairlayDotNetClient.Private.Responses;

namespace FairlayDotNetClient.Tests.Private
{
	public static class TestData
	{
		public const string ClientPrivateRsaXml =
			"<RSAKeyValue><Modulus>5ZuQEg2qtTrYvehQTxdBJiOSyNiBpBW+fVlk7HJ9+6OQmGCNhnjLfcGFMA1ODxsiC" +
			"8ILlI5++l2H8SCtDLNMGi2O5aDBsY9Bs+QetQAJVKyFtsoDRtBvy4vlR/joMP+jic/Mt2r92riFGWunZliPI47q" +
			"iPVJd8JycXR2Kf8VbCU=</Modulus><Exponent>AQAB</Exponent><P>/b6BwxydWyLh1RAaJtO4ROiRNYUB1" +
			"4TAzjfleylapPfT64HlDyLbMzolg0zbH1FDq6D17mEB+Ac3m9+cKHWpzQ==</P><Q>56Yfum0pgh4Yi0WKzmiu/" +
			"rKYVkVqjz0acjczKtgaUjLEsE2vUM4B9N+8S2EXci5TZ/dCsYktMuD157Ea5sGTuQ==</Q><DP>sR4zka/9nsoA" +
			"fSraNlP/AgqcoZEQMhH2S3v08T1yikh7Yp6u9xvLijyCRt507a1Z4Qlf4V9RcoIHLQSvCgbn2Q==</DP><DQ>01" +
			"Bv0RiTrObXbPTbUr+cIyu4W7qnIlOTNG22d3b7S9CULGAxdXz3u/H9SqYfRUGNRGICrQF+AdPFfr3I1IfxwQ==<" +
			"/DQ><InverseQ>dA/bkeMYF3YO1+2ZQftJ5M1tcyBocrYu30q3ztNwqegWwKmBNQc+GxM/OY0ybTQBkuwX3IMo8" +
			"0KUgHj7puOgfw==</InverseQ><D>oAyvAZabPxcVDFPK30bTd/VmFTCuNaWekhvlONiaLvWWDlGHdYvwNOnXoG" +
			"Y12lvMgKuzjMtDgdv+rbtcRTUaPvbW14ZBLQ0FakuNtOwCax8uGGSeFsmBoLXJXTWJdtrlCyV1N581X+7yfeKjE" +
			"+xXT1KLT0ojUCOqEq/yEQtZMeE=</D></RSAKeyValue>";

		public static readonly RSAParameters ClientPrivateRsaParameters =
			RsaParametersExtensions.CreateFromXmlString(ClientPrivateRsaXml);

		public const string ServerPublicRsaXml =
			"<RSAKeyValue><Modulus>udnE0+F2lSFLJs3wyQT/2W53juqh1hW9NaEwWMfefkV8FHUJTgJQINBrvja/Ii6i1" +
			"W2ptBhNjin63K0stJmFArdi74TTL0KoTlBpZ3x0r4SQZGX+ZoryO5NFa4UB7NbYvKJxZHnjnFJiNtnf08rOmgdt" +
			"DbukHnaVm7m067V+dyk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

		public static readonly RSAParameters ServerPublicRsaParameters =
			RsaParametersExtensions.CreateFromXmlString(ServerPublicRsaXml);

		public static readonly PrivateApiRequest ApiRequest = new PrivateApiRequest(UserId,
			NamedRequestHeader, RequestBody);

		private static readonly byte[] RequestSignature =
			Convert.FromBase64String("k6kkNa2eux2/fIun+CaCmSQGzdwbuA20PGuePP6rV425T7UF8bywN/GrA7cIyE" +
				"+8azVe3HJEx/DHDmCOD5X4skZZLeDjgpWvf+k3OSJmKz4KWU/fAG9x8bXMzbsBBJrA/kQADQMYlasKwKxdoJs" +
				"7mxlsKrf9TxD1ZmSsCWjhgz8=");

		public static readonly SignedPrivateApiRequest SignedApiRequest =
			new SignedPrivateApiRequest(ApiRequest, RequestSignature, RequestNonce);

		public const long UserId = 1007206;
		public const int NativeApiAccountId = 0;
		public const int RegisteredApiAccountId = 1;
		public const string NumericRequestHeader = "25";
		public const string NamedRequestHeader = nameof(NamedRequestHeader);
		public const string RequestBody = nameof(RequestBody);
		public const long RequestNonce = 636395926873808918;

		private static readonly byte[] ResponseSignature =
			Convert.FromBase64String("Sv/ugypguTYdyOI3Ul5OmyfohGnIi3z88Acsesp2HwLjSJP70mK9w+ev7Le1Mq" +
				"unOIzTHmgI3MWbDJeWM9IuSZxUebqG3HvSYsHwKkJKmh2NgOk7auZAu3dkfs6XUuq1BoRWBTeq0Wy2Ik7MGVE" +
				"Wnh1FLvsfHSYeRZJwyAqgm08=");

		public static readonly PrivateApiResponse ApiResponse =
			new PrivateApiResponse(ResponseSignature, ResponseNonce, ResponseServerId, ResponseBody);

		private const long ResponseNonce = 636396908273886242;
		private const int ResponseServerId = 66;
		private const string ResponseBody = "636396908276292303";

		public static readonly IPEndPoint ApiServerEndPoint =
			new IPEndPoint(IPAddress.Parse("31.172.83.53"), 18017);
	}
}