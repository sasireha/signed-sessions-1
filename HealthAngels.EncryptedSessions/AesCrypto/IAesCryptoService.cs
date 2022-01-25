namespace HealthAngels.EncryptedSessions.AesCrypto
{
    public interface IAesCryptoService
    {
        byte[] DecryptAESGCM(byte[] cipherText, byte[] key, byte[] nonce, byte[] tag, byte[] associatedData = null);
        byte[] EncryptAESGCM(byte[] plainText, byte[] key, byte[] nonce, byte[] tag, byte[] associatedData = null);
        AesCryptoData MakeAesCryptoData(byte[] data, byte[] nonce, byte[] tag);
    }
}
