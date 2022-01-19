using Microsoft.Extensions.Options;
using System;
using System.Security.Cryptography;
using System.Text;

namespace HealthAngels.EncryptedSessions.Signature
{
    public class SignatureHelper : ISignatureHelper
    {
        public Encoding Encoding => Encoding.UTF8;

        private readonly SignatureSecrets _signatureSecrets;

        public SignatureHelper(IOptions<SignatureSecrets> signatureSecrets)
        {
            _signatureSecrets = signatureSecrets.Value;
        }

        public string CreateSignature(byte[] message)
        {
            if (message == null)
                return string.Empty;

            byte[] keyByte = Encoding.GetBytes(_signatureSecrets.HmacSecretKey);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(message);
                return Convert.ToBase64String(hashmessage);
            }
        }

        public bool VerifySignature(string unsignedValue, string signature)
        {
            string computedSignature;
            if (!string.IsNullOrEmpty(unsignedValue) && !string.IsNullOrEmpty(signature))
            {
                computedSignature = CreateSignature(Convert.FromBase64String(unsignedValue));
                if (computedSignature.Equals(signature))
                    return true;
            }

            return false;
        }
    }
}
