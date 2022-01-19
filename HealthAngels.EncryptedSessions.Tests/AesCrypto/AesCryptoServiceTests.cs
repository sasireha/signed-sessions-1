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

        [Fact]
        public void DecryptAESGCM_Should_Return_PlainText_WardenTokens()
        {
            // Arrange
            var key = "keykeykeykeykeykeykeykeykeykeyke"; //  must be 256 bit (=32 bytes) (=AES256)
            var nonceBase64 = "zR2qWpdOTaZVCH77"; // must be 12 bytes
            var cipherTextBase64 = "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";
            var cipherTagBase64 = "40NGWAUc6t1RtvE0hGenIA==";

            var cipherTextBytes = Convert.FromBase64String(cipherTextBase64);
            var tagBytes = Convert.FromBase64String(cipherTagBase64);
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var nonceBytes = Convert.FromBase64String(nonceBase64);
            var expectedPlainText = "{\"accessToken\":\"eyJhbGciOiJSUzI1NiIsImtpZCI6IjA3QTZDOTQ4NzZDQ0Y0RTZBMzBFRTFBMzNBMUU3NkIzMUI4RTQ2QTMiLCJ0eXAiOiJhdCtqd3QiLCJ4NXQiOiJCNmJKU0hiTTlPYWpEdUdqT2g1MnN4dU9ScU0ifQ.eyJuYmYiOjE2MTQ2MTU1MjAsImV4cCI6MTYxNDYxOTEyMCwiaXNzIjoiaHR0cHM6Ly9sb2NhbC50NXQubmwvaWRlbnRpdHkvYXBpIiwiYXVkIjoiZmhpci9wYXRpZW50IiwiY2xpZW50X2lkIjoid2FyZGVuIiwic3ViIjoiZ3BkYXRhLXBhdGllbnQta3dhbGlmaWNhdGllMSIsImF1dGhfdGltZSI6MTYxNDYxNTUxOSwiaWRwIjoibG9jYWwiLCJzY29wZSI6WyJvcGVuaWQiLCJwcm9maWxlIiwiZW1haWwiLCJwYXRpZW50LyouKiJdLCJhbXIiOlsicHdkIl19.HM0h151Hl_9nvJwjw81cqEaECAqXo5_D_7HU41gBC0OCu6ym6VOozlAPtzXyqcy1xis-27kpA_Cjb39zPxiOV-L8BfvqLZyfb3sIZXrDb0MMWwlGB0NjHiMbZvIjWFJ0MDsmHygRGcNh7p_lBhDTjh6ZNcwDC1SG_ski7WnkdbiATt_0nvOv49VUKZqnO5YReGHBJLl_owvROZgGYbmbPatu1yXttbOJjLQGHkl_pmIiSMRHQE6fSW3kWvczZLpjEQVLFdmCtGoainzoTCIQJojshSJOepPL2v11u4RfxhKIu53fZbjgvrDQADnz4JTvSRUiduiMP_-J9ic6xjtf3A\",\"refreshToken\":\"\",\"idToken\":\"eyJhbGciOiJSUzI1NiIsImtpZCI6IjA3QTZDOTQ4NzZDQ0Y0RTZBMzBFRTFBMzNBMUU3NkIzMUI4RTQ2QTMiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJCNmJKU0hiTTlPYWpEdUdqT2g1MnN4dU9ScU0ifQ.eyJuYmYiOjE2MTQ2MTU1MjAsImV4cCI6MTYxNDYxOTEyMCwiaXNzIjoiaHR0cHM6Ly9sb2NhbC50NXQubmwvaWRlbnRpdHkvYXBpIiwiYXVkIjoid2FyZGVuIiwiaWF0IjoxNjE0NjE1NTIwLCJhdF9oYXNoIjoia3E5U19pRkk0WWh3RlFWbmFVdktadyIsInNfaGFzaCI6IkFFR1Q3N3VBWGlZbERSUTA2N0J1Q0EiLCJzaWQiOiJ6eXhTckJpaEs2Qm9Za2tweTBFdHNnIiwic3ViIjoiZ3BkYXRhLXBhdGllbnQta3dhbGlmaWNhdGllMSIsImF1dGhfdGltZSI6MTYxNDYxNTUxOSwiaWRwIjoibG9jYWwiLCJuYW1lIjoiQWxpY2UgU21pdGgiLCJnaXZlbl9uYW1lIjoiQWxpY2UiLCJmYW1pbHlfbmFtZSI6IlNtaXRoIiwiZ2VuZGVyIjoiRiIsImJpcnRoZGF0ZSI6IjIwLzAzLzE5ODgiLCJ6b25laW5mbyI6IkV1cm9wZS9BbXN0ZXJkYW0iLCJsb2NhbGUiOiJubF9OTCIsInByZWZlcnJlZF91c2VybmFtZSI6ImFsaWNlQGVtYWlsLmNvbSIsImVtYWlsIjoiYWxpY2VAZW1haWwuY29tIiwiZW1haWxfdmVyaWZpZWQiOnRydWUsImFtciI6WyJwd2QiXX0.n4Npaumam-KhVyTKnenBXb_M5TnC5E87EDEzE3Q-Bk3wUuETK2ZURHfxMqQJX1B3bEvJubOpU6cTceRpv-twI1Rju1cZV5S7jb_w2STN-cRoxDJOjQ5itll3EiS1xsyelRgP4V5XTrJtBLp9WV-EGPtF5imoSWFts0zhLhrpeIIM9ULA33d-5qh7C03l_DqkEoWk20a_EX2DpzeT52h9wkLKDN4N5tBpWx7wI-DSrIJ1RCcLcxN4bfjBGPzAu4Y_QzmbRVyhRtaKh33s5gpgKTzcB0NqyBpDkXdet32zA-2gXNyxXSh0aG5-lyFmOuQwFxzi7o5Nai_GEkgvSNnHZQ\",\"expiry\":\"2021-03-01T17:18:40.9028619Z\"}";

            // Act 
            byte[] decodedBytes = _aesCrypto.DecryptAESGCM(cipherTextBytes, keyBytes, nonceBytes, tagBytes);
            string plaintext = Encoding.UTF8.GetString(decodedBytes);

            // Assert
            Assert.Equal(expectedPlainText.Length, plaintext.Length);
            Assert.Equal(expectedPlainText, plaintext);
        }
    }
}
