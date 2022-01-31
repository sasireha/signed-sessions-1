namespace HealthAngels.EncryptedSessions.AesCrypto
{
    public class AesCryptoData
    {
        public AesCryptoData(string cypherdata, string nonce, string tag)
        {
            CypherData = cypherdata;
            Nonce = nonce;
            Tag = tag;
        }
        public string CypherData { get; }
        public string Nonce { get; }
        public string Tag { get; }
    }
}
