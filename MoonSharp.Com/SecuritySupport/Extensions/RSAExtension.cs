using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace MoonSharp.Extensions
{
    public static partial class SecuritySupport_RSAExtentions
    {
        public static void FromXmlStringCore(this RSA rsa, string xmlString)
        {
            var @params = new RSAParameters();
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
            {
                switch (node.Name)
                {
                    case "Modulus": @params.Modulus = Convert.FromBase64String(node.InnerText); break;
                    case "Exponent": @params.Exponent = Convert.FromBase64String(node.InnerText); break;
                    case "P": @params.P = Convert.FromBase64String(node.InnerText); break;
                    case "Q": @params.Q = Convert.FromBase64String(node.InnerText); break;
                    case "DP": @params.DP = Convert.FromBase64String(node.InnerText); break;
                    case "DQ": @params.DQ = Convert.FromBase64String(node.InnerText); break;
                    case "InverseQ": @params.InverseQ = Convert.FromBase64String(node.InnerText); break;
                    case "D": @params.D = Convert.FromBase64String(node.InnerText); break;
                }
            }

            rsa.ImportParameters(@params);
        }

        public static string ToXmlStringCore(this RSA rsa, bool includePrivateParameters)
        {
            var @params = rsa.ExportParameters(includePrivateParameters);

            if (includePrivateParameters)
            {
                return $"<RSAKeyValue>\r\n" +
                    $"<Modulus>{Convert.ToBase64String(@params.Modulus)}</Modulus>\r\n" +
                    $"<Exponent>{Convert.ToBase64String(@params.Exponent)}</Exponent>\r\n" +
                    $"<P>{Convert.ToBase64String(@params.P)}</P>\r\n" +
                    $"<Q>{Convert.ToBase64String(@params.Q)}</Q>\r\n" +
                    $"<DP>{Convert.ToBase64String(@params.DP)}</DP>\r\n" +
                    $"<DQ>{Convert.ToBase64String(@params.DQ)}</DQ>\r\n" +
                    $"<InverseQ>{Convert.ToBase64String(@params.InverseQ)}</InverseQ>\r\n" +
                    $"<D>{Convert.ToBase64String(@params.D)}</D>\r\n" +
                    $"</RSAKeyValue>";
            }
            else
            {
                return $"<RSAKeyValue>\r\n" +
                    $"<Modulus>{Convert.ToBase64String(@params.Modulus)}</Modulus>\r\n" +
                    $"<Exponent>{Convert.ToBase64String(@params.Exponent)}</Exponent>\r\n" +
                    $"</RSAKeyValue>";
            }
        }

        public static void FromPemStringCore(this RSA rsa, string pemString, bool includePrivateParameters)
        {
            rsa.ImportParameters(GetParametersFromPem(pemString, includePrivateParameters));
        }

        public static string ToPemStringCore(this RSA rsa, bool includePrivateParameters)
        {
            var @params = rsa.ExportParameters(includePrivateParameters);

            if (includePrivateParameters)
            {
                var tree =
                    Sequence(
                        Integer(new byte[] { 0x00 }),
                        Sequence(RSA_OID),
                        OctetString(
                            Sequence(
                                Integer(new byte[] { 0x00 }),
                                Integer(@params.Modulus),
                                Integer(@params.Exponent),
                                Integer(@params.D),
                                Integer(@params.P),
                                Integer(@params.Q),
                                Integer(@params.DP),
                                Integer(@params.DQ),
                                Integer(@params.InverseQ))));

                return "-----BEGIN PRIVATE KEY-----\r\n" +
                    string.Join("\r\n", Convert.ToBase64String(tree)
                    .ToCharArray().GroupArray(64).Select(a => new string(a)))
                    + "\r\n-----END PRIVATE KEY-----";
            }
            else
            {
                var tree =
                    Sequence(
                        Sequence(RSA_OID),
                        BitString(
                            new byte[] { 0x00 },
                            Sequence(
                                Integer(@params.Modulus),
                                Integer(@params.Exponent))));

                return "-----BEGIN PUBLIC KEY-----\r\n" +
                    string.Join("\r\n", Convert.ToBase64String(tree)
                    .ToCharArray().GroupArray(64).Select(a => new string(a)))
                    + "\r\n-----END PUBLIC KEY-----";
            }
        }

    }

}
