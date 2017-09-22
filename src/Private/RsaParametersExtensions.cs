using System;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Linq;

namespace FairlayDotNetClient.Private
{
	public static class RsaParametersExtensions
	{
		public static RSAParameters FromXmlString(string xmlString)
		{
			var rsaParameters = new RSAParameters();
			var xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(xmlString);
			if (xmlDoc.DocumentElement.Name == RsaKeyValueElementName)
				foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
					rsaParameters = ImportRsaParameterFromXmlNode(node, rsaParameters);
			else
				throw new CryptographicException("Invalid XML RSA key");
			return rsaParameters;
		}

		private const string RsaKeyValueElementName = "RSAKeyValue";

		// ReSharper disable once MethodTooLong
		private static RSAParameters ImportRsaParameterFromXmlNode(XmlNode node,
			RSAParameters rsaParameters)
		{
			switch (node.Name)
			{
				case nameof(rsaParameters.D):
					rsaParameters.D = Convert.FromBase64String(node.InnerText);
					break;
				case nameof(rsaParameters.DP):
					rsaParameters.DP = Convert.FromBase64String(node.InnerText);
					break;
				case nameof(rsaParameters.DQ):
					rsaParameters.DQ = Convert.FromBase64String(node.InnerText);
					break;
				case nameof(rsaParameters.Exponent):
					rsaParameters.Exponent = Convert.FromBase64String(node.InnerText);
					break;
				case nameof(rsaParameters.InverseQ):
					rsaParameters.InverseQ = Convert.FromBase64String(node.InnerText);
					break;
				case nameof(rsaParameters.Modulus):
					rsaParameters.Modulus = Convert.FromBase64String(node.InnerText);
					break;
				case nameof(rsaParameters.P):
					rsaParameters.P = Convert.FromBase64String(node.InnerText);
					break;
				case nameof(rsaParameters.Q):
					rsaParameters.Q = Convert.FromBase64String(node.InnerText);
					break;
			}
			return rsaParameters;
		}

		public static string ToXmlString(this RSAParameters rsaParameters)
		{
			var rsaElement = new XElement(RsaKeyValueElementName);
			AddRsaParameterToXContainer(rsaElement, nameof(rsaParameters.D), rsaParameters.D);
			AddRsaParameterToXContainer(rsaElement, nameof(rsaParameters.DP), rsaParameters.DP);
			AddRsaParameterToXContainer(rsaElement, nameof(rsaParameters.DQ), rsaParameters.DQ);
			AddRsaParameterToXContainer(rsaElement, nameof(rsaParameters.Exponent), rsaParameters.Exponent);
			AddRsaParameterToXContainer(rsaElement, nameof(rsaParameters.InverseQ), rsaParameters.InverseQ);
			AddRsaParameterToXContainer(rsaElement, nameof(rsaParameters.Modulus), rsaParameters.Modulus);
			AddRsaParameterToXContainer(rsaElement, nameof(rsaParameters.P), rsaParameters.P);
			AddRsaParameterToXContainer(rsaElement, nameof(rsaParameters.Q), rsaParameters.Q);
			return new XDocument(rsaElement).ToString();
		}

		private static void AddRsaParameterToXContainer(XContainer container, string rsaParameterName,
			byte[] rsaParameterData)
		{
			if (rsaParameterData != null)
				container.Add(new XElement(rsaParameterName, Convert.ToBase64String(rsaParameterData)));
		}
	}
}