namespace HealthAngels.EncryptedSessions.AesCrypto
{
    public record AesCryptoData
    {
        public string CypherData { get; init; }
        public string Nonce { get; init; }
        public string Tag { get; init; }
    }
}
