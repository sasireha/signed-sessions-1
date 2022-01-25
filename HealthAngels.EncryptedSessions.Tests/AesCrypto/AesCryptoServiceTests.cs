using HealthAngels.EncryptedSessions.AesCrypto;
using System.Text;
using Xunit;

namespace HealthAngels.EncryptedSessions.Tests.AesCrypto
{
    public class AesCryptoServiceTests
    {
        private readonly IAesCryptoService _aesCrypto;

        public AesCryptoServiceTests()
        {
            _aesCrypto = new AesCryptoService();
        }

        [Fact]
        public void Should_Encrypt_And_Decrypt_GivenData()
        {
            var plainText = "very secret and sensitive token information";
            var key = "keykeykeykeykeykeykeykeykeykeyke"; //  must be 32 bytes (AES256)
            var nonce = "iviviviviviv"; // must be 12 bytes

            var plainData = Encoding.UTF8.GetBytes(plainText);
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var nonceBytes = Encoding.UTF8.GetBytes(nonce);
            var tagBytes = new byte[16]; // must be 16 bytes

            //Encrypt
            var cipherTextBytes = _aesCrypto.EncryptAESGCM(plainData, keyBytes, nonceBytes, tagBytes);
            Assert.NotNull(cipherTextBytes);
            Assert.NotNull(tagBytes);

            //Decrypt
            var decodedBytes = _aesCrypto.DecryptAESGCM(cipherTextBytes, keyBytes, nonceBytes, tagBytes);
            Assert.Equal(plainData, decodedBytes);         
        }

        [Fact]
        public void EncryptAESGCM_WhenKeyLength_NotSatisfied_ReturnsNull()
        {
            var plainText = "very secret and sensitive token information";
            var key = "SomeKey"; //  must be 32 bytes (AES256)
            var nonce = "iviviviviviv"; // must be 12 bytes

            var plainData = Encoding.UTF8.GetBytes(plainText);
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var nonceBytes = Encoding.UTF8.GetBytes(nonce);
            var tagBytes = new byte[16]; // must be 16 bytes

            //Encrypt
            var cipherTextBytes = _aesCrypto.EncryptAESGCM(plainData, keyBytes, nonceBytes, tagBytes);
            Assert.Null(cipherTextBytes);
        }

        [Fact]
        public void EncryptAESGCM_WhenNonceLength_NotSatisfied_ReturnsNull()
        {
            var plainText = "very secret and sensitive token information";
            var key = "keykeykeykeykeykeykeykeykeykeyke"; //  must be 32 bytes (AES256)
            var nonce = "somenonce"; // must be 12 bytes

            var plainData = Encoding.UTF8.GetBytes(plainText);
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var nonceBytes = Encoding.UTF8.GetBytes(nonce);
            var tagBytes = new byte[16]; // must be 16 bytes

            //Encrypt
            var cipherTextBytes = _aesCrypto.EncryptAESGCM(plainData, keyBytes, nonceBytes, tagBytes);
            Assert.Null(cipherTextBytes);
        }
    }
}
