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
			"<RSAKeyValue><Modulus>o64bABZ0Vs53iA473sKBPCkelOoi89DO7cU6Q7BvyKXJ+8iFjPipV6VNIadm8IlRA" +
			"l0ccUQrJ3D4v21nAAUApn2KRCOVFa+XKcoEe4OW4vqa3lVdCgltVPBsuLt6heD8ZRxO7sHIAd9q3PgQfihvUsCm" +
			"c9GR4d3sEUfaXP3kkgj0ZEVfAtgcEMYSVH9tKpCtGBKAoA1bQ47aHOJMr/iccURqq6kuBqZlwt1XiCp+7cf0V71" +
			"u2ySzi7k+n2UJzxuiWpBi6PXH/hwzY2MXukXrKOGjL4qegiRSu9p7ZbWZRtx0ia5m4sSRq2GLZzYG3ht3X/4H0k" +
			"DZam1h5Ieqt0JWsw==</Modulus><Exponent>AQAB</Exponent><P>1D35jZ4b3Vx87eEwJec5PwJaYFhIkR7" +
			"Vggg3HWt/7Res7s1ZphGOo7qK3M4bq+swy8ero0cTsKXHPpHMNPlepZ0VZMHY4sQPTRNqP1xHIDa+KzbNb2mqZd" +
			"BAhH0ToxQdlgIdz2IE9gbcbga7nPTD4n2N3jCDhtloEFkNlNj8gOE=</P><Q>xW0N9bRTMzuu69+3ju2wbCu7uQ" +
			"9iH5clQFW0/8iMtn8naq2arXEeqKARTx+JLEl5MkJAayJy4IEOxWeXqlJAMxFmNxkIz6KojlupLxveBViwUPb6z" +
			"FKmPBEv+vUOkb/IrYkMiA6oCYTgccjNadhXMy8tf4E3eiCiz9FLrv/vhhM=</Q><DP>VLQDI1SlVDu0FA+Z8paM" +
			"Dyx2OxsgouIGXaW/sTsUy992Abvsek1peshY4PRUsNDlIX6nbjtQjWAr2zm+oKmg70F4p0SBtUZ/wMft0CmYrS+" +
			"Lx51QcZVfDtEE3ps4Og4uHI7trLU2u6VTVYnUWDiK8JohAzqjUy7yzAtuUygh5aE=</DP><DQ>AX6BzIpA1vJss" +
			"HxQ91P1Mdxi31Ouem66aXI8nHL4Hal4suX9dbKVeNE9UEv8zckAkdDOjAEGvYw8gGH4U+Grerd2/pPB7VBd1jX7" +
			"Yc/kLkYtj29PFpzJhUrcAho4sgGxkx6maaEyxF+tEy9h3ps0jb6dqYlg0hvNH6WF3R0ywuE=</DQ><InverseQ>" +
			"W7rdXXBoJVKbMRVPdsgdT473+beXtbDBAVlGxjyws1XQ3vzIWcdY8rwIfEII29Q6MZG5ryNEE/DbE5iwPuqBXUO" +
			"FgCpnt/98K+oMaq9yEAoA1zkwgWppKCNGPMP72wgOgE71PgFfcNsUlXtkFty7Zfb2p1OLBfEy/t6e3dqkPhA=</" +
			"InverseQ><D>CJtBBDIqD95vXpIRB9eGYBlxaDcfe16BYvM56xE973j4M+sJSq5QiTFB6wEYYpF11v/6XOJOupY" +
			"wHjCsd83LiPWU7ZxJXIiNFD9xGOWEa47t0b7/U2XHf+3caaJCAQQhgVeJtvgtghuzTMcmZtvNbrZ+ihx85Tl8Dj" +
			"SPANDA0gTc+lyMTqxXVBIX43rY0Hglm54P92EIwsUEsp/TQ1rXDPVTkhIQY1wXv+mdV1SrvZQbvV/DCHZeLJveG" +
			"5h4Gc3X9Npq1QytTfaSwV/m8Hk0qqrpZChKdUh6RiHIg7g+UeUsElXzUfZ3Un6gHyuTk1FH0dsxguBGzzH+zEFg" +
			"W1SCgQ==</D></RSAKeyValue>";

		public const string ClientPublicRsaXml =
			"<RSAKeyValue><Modulus>o64bABZ0Vs53iA473sKBPCkelOoi89DO7cU6Q7BvyKXJ+8iFjPipV6VNIadm8IlRA" +
			"l0ccUQrJ3D4v21nAAUApn2KRCOVFa+XKcoEe4OW4vqa3lVdCgltVPBsuLt6heD8ZRxO7sHIAd9q3PgQfihvUsCm" +
			"c9GR4d3sEUfaXP3kkgj0ZEVfAtgcEMYSVH9tKpCtGBKAoA1bQ47aHOJMr/iccURqq6kuBqZlwt1XiCp+7cf0V71" +
			"u2ySzi7k+n2UJzxuiWpBi6PXH/hwzY2MXukXrKOGjL4qegiRSu9p7ZbWZRtx0ia5m4sSRq2GLZzYG3ht3X/4H0k" +
			"DZam1h5Ieqt0JWsw==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

		public static readonly RSAParameters ClientPublicRsaParameters =
			RsaParametersExtensions.FromXmlString(ClientPublicRsaXml);

		public const string ServerPublicRsaXml =
			"<RSAKeyValue><Modulus>udnE0+F2lSFLJs3wyQT/2W53juqh1hW9NaEwWMfefkV8FHUJTgJQINBrvja/Ii6i1" +
			"W2ptBhNjin63K0stJmFArdi74TTL0KoTlBpZ3x0r4SQZGX+ZoryO5NFa4UB7NbYvKJxZHnjnFJiNtnf08rOmgdt" +
			"DbukHnaVm7m067V+dyk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

		public static readonly RSAParameters ServerPublicRsaParameters =
			RsaParametersExtensions.FromXmlString(ServerPublicRsaXml);

		public static readonly PrivateApiCredentials Credentials = new PrivateApiCredentials
		{
			UserId = 1007206,
			ApiAccountId = 1,
			PrivateRsaParameters = RsaParametersExtensions.FromXmlString(ClientPrivateRsaXml),
			ServerEndPoint = new IPEndPoint(IPAddress.Parse("31.172.83.53"), 18017)
		};

		public static readonly PrivateApiRequest ApiRequest = new PrivateApiRequest(Credentials.UserId,
			NumericRequestHeader, RequestBody);

		private const string NumericRequestHeader = "25";
		private const string RequestBody = nameof(RequestBody);
		public const string NamedRequestHeader = nameof(NamedRequestHeader);

		private static readonly byte[] RequestSignature =
			Convert.FromBase64String("k6kkNa2eux2/fIun+CaCmSQGzdwbuA20PGuePP6rV425T7UF8bywN/GrA7cIyE" +
				"+8azVe3HJEx/DHDmCOD5X4skZZLeDjgpWvf+k3OSJmKz4KWU/fAG9x8bXMzbsBBJrA/kQADQMYlasKwKxdoJs" +
				"7mxlsKrf9TxD1ZmSsCWjhgz8=");

		public static readonly SignedPrivateApiRequest SignedApiRequest =
			new SignedPrivateApiRequest(ApiRequest, RequestSignature, RequestNonce);

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

		/// <summary>
		/// Normal users do not have the private key to use their native API Account #0.
		/// </summary>
		public const int NativeApiAccountId = 0;
	}
}