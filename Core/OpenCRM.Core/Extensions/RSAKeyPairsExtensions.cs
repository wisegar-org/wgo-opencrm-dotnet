using System.Security.Cryptography;
using System.Xml.Serialization;
using OpenCRM.Core.Crypto;

namespace OpenCRM.Core.Extensions;

public static class RSAKeyPairsExtensions
{
    public static string GetStringPublicKey(this RSAKeyPairsModel rsaKeyPairs)
    {
        var sw = new StringWriter();
        var xmlSerializer = new XmlSerializer(typeof(RSAParameters));
        xmlSerializer.Serialize(sw, rsaKeyPairs.PublicKey);
        return sw.ToString();
    }
    public static string GetStringPrivateKey(this RSAKeyPairsModel rsaKeyPairs)
    {
        var sw = new StringWriter();
        var xmlSerializer = new XmlSerializer(typeof(RSAParameters));
        xmlSerializer.Serialize(sw, rsaKeyPairs.PrivateKey);
        return sw.ToString();
    }
}