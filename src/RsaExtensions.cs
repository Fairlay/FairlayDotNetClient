using System;
using System.Security.Cryptography;
using System.Xml;

namespace FairlayDotNetClient
{
	public static class RsaExtensions
	{
		/// <summary>
		/// Build-in FromXmlString does not work (PlatformNotSupportedException)
		/// </summary>
		public static void ImportFromXmlString(this RSA rsa, string xmlString)
		{
			var parameters = new RSAParameters();
			var xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(xmlString);
			if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
				foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
					parameters = ImportRsaParameterFromXmlNode(node, parameters);
			else
				throw new CryptographicException("Invalid XML RSA key.");
			rsa.ImportParameters(parameters);
		}

		// ReSharper disable once MethodTooLong
		private static RSAParameters ImportRsaParameterFromXmlNode(XmlNode node,
			RSAParameters parameters)
		{
			switch (node.Name)
			{
				case nameof(parameters.Modulus):
					parameters.Modulus = Convert.FromBase64String(node.InnerText);
					break;
				case nameof(parameters.Exponent):
					parameters.Exponent = Convert.FromBase64String(node.InnerText);
					break;
				case nameof(parameters.P):
					parameters.P = Convert.FromBase64String(node.InnerText);
					break;
				case nameof(parameters.Q):
					parameters.Q = Convert.FromBase64String(node.InnerText);
					break;
				case nameof(parameters.DP):
					parameters.DP = Convert.FromBase64String(node.InnerText);
					break;
				case nameof(parameters.DQ):
					parameters.DQ = Convert.FromBase64String(node.InnerText);
					break;
				case nameof(parameters.InverseQ):
					parameters.InverseQ = Convert.FromBase64String(node.InnerText);
					break;
				case nameof(parameters.D):
					parameters.D = Convert.FromBase64String(node.InnerText);
					break;
			}
			return parameters;
		}
	}
}