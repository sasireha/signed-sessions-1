using System;
using Xunit;
using System.Text;
using HealthAngels.EncryptedSessions.AesCrypto;

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
        public void EncryptAESGCM_Should_Return_CypherText()
        {
            // Arrange
            var plainText = "very secret and sensitive token information";
            var key = "keykeykeykeykeykeykeykeykeykeyke"; //  must be 32 bytes (AES256)
            var nonce = "iviviviviviv"; // must be 12 bytes
            var expectedCipherTextBase64 = "tbqYx/IHCNSEUxyJKoFXNiyVqAuIskn7g24q/uA1q+PmXalNBmrndVs9Jw==";
            var expectedCipherTagBase64 = "gb/1xQUY0P+P//D7fkyGBg==";

            var plainData = Encoding.UTF8.GetBytes(plainText);
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var nonceBytes = Encoding.UTF8.GetBytes(nonce);
            var tagBytes = new byte[16]; // must be 16 bytes

            // Act 
            var cipherTextBytes = _aesCrypto.EncryptAESGCM(plainData, keyBytes, nonceBytes, tagBytes);

            // Assert
            Assert.Equal(plainData.Length, cipherTextBytes.Length);
            Assert.Equal(expectedCipherTextBase64, Convert.ToBase64String(cipherTextBytes));
            Assert.Equal(expectedCipherTagBase64, Convert.ToBase64String(tagBytes));
        }

        [Fact]
        public void DecryptAESGCM_Should_Return_PlainText()
        {
            // Arrange
            var key = "keykeykeykeykeykeykeykeykeykeyke"; //  must be 256 bit (=32 bytes) (=AES256)
            var nonce = "iviviviviviv"; // must be 12 bytes
            var cipherTextBase64 = "tbqYx/IHCNSEUxyJKoFXNiyVqAuIskn7g24q/uA1q+PmXalNBmrndVs9Jw==";
            var cipherTagBase64 = "gb/1xQUY0P+P//D7fkyGBg==";

            var cipherTextBytes = Convert.FromBase64String(cipherTextBase64);
            var tagBytes = Convert.FromBase64String(cipherTagBase64);
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var nonceBytes = Encoding.UTF8.GetBytes(nonce);
            var expectedPlainText = "very secret and sensitive token information";

            // Act 
            var decodedBytes = _aesCrypto.DecryptAESGCM(cipherTextBytes, keyBytes, nonceBytes, tagBytes);
            var plaintext = Encoding.UTF8.GetString(decodedBytes);

            // Assert
            Assert.Equal(expectedPlainText.Length, plaintext.Length);
            Assert.Equal(expectedPlainText, plaintext);
        }
    }
}
