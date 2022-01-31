using System;
using System.Security.Cryptography;

namespace HealthAngels.EncryptedSessions.AesCrypto
{
    public class AesCryptoService : IAesCryptoService
    {
        // Important
        // size of key must be 32 bytes for AES-256 ( can be 16,24 and 32 ) (https://en.wikipedia.org/wiki/Advanced_Encryption_Standard)
        // size of nonce (=iv short for initialization vector) must be 12 bytes (https://en.wikipedia.org/wiki/Initialization_vector)
        // add (= Additional Authenticated Data) can be nil
        public byte[] EncryptAESGCM(byte[] plainText, byte[] key, byte[] nonce, byte[] tag, byte[] associatedData = null)
        {
            if (!ValidateInput(plainText, key, nonce, tag)) { return null; }

            byte[] cipherText = new byte[plainText.Length];
            using var cipher = new AesGcm(key);
            cipher.Encrypt(nonce, plainText, cipherText, tag, associatedData);
            return cipherText;
        }

        public byte[] DecryptAESGCM(byte[] cipherText, byte[] key, byte[] nonce, byte[] tag, byte[] associatedData = null)
        {
            if (!ValidateInput(cipherText, key, nonce, tag)) { return null; }

            byte[] plainText = new byte[cipherText.Length];
            using var cipher = new AesGcm(key);
            cipher.Decrypt(nonce, cipherText, tag, plainText, associatedData);
            return plainText;
        }

        public AesCryptoData MakeAesCryptoData(byte[] data, byte[] nonce, byte[] tag)
        {
            return new AesCryptoData(Convert.ToBase64String(data), Convert.ToBase64String(nonce), Convert.ToBase64String(tag));
        }

        private bool ValidateInput(byte[] text, byte[] key, byte[] nonce, byte[] tag)
        {
            if (text == null || text.Length == 0) { return false; }

            if (tag == null || tag.Length > 16 || tag.Length < 12 || nonce == null || tag.Length != 16) { return false; }

            if (nonce == null || nonce.Length != 12) { return false; }

            if (key == null || key.Length != 32) { return false; }

            return true;
        }
    }
}
