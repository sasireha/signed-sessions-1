namespace HealthAngels.EncryptedSessions.AesCrypto
{
    public class AesCryptoData
    {
        public string CypherData { get; set; }
        public string Nonce { get; set; }
        public string Tag { get; set; }
    }
}
