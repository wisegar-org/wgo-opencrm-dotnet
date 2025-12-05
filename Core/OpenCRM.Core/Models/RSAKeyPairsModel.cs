using System.Security.Cryptography;
using System.Xml.Serialization;

namespace OpenCRM.Core.Crypto
{
    public class RSAKeyPairsModel
    {
        public RSAParameters PublicKey { get; set; }
        public RSAParameters PrivateKey { get; set; }
    }


}
