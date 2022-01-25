using HealthAngels.EncryptedSessions.AesCrypto;
using HealthAngels.EncryptedSessions.Cache;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HealthAngels.EncryptedSessions.Tests.Cache
{
    public class EncryptedDistributedCacheTests
    {
        private readonly Mock<IDistributedCache> _baseCacheMock;
        private readonly IAesCryptoService _aesCryptoService;
        private readonly Mock<IOptions<AesCryptoConfig>> _config;

        private readonly EncryptedDistributedCache _encryptedDistributedCache;
        private string _key = Guid.NewGuid().ToString();

        private DistributedCacheEntryOptions _distributedCacheEntryOptions = new DistributedCacheEntryOptions();

        //Encryption
        private byte[] _decryptedValue = Encoding.UTF8.GetBytes("teststring");
        private byte[] _encryptedCacheData = Encoding.UTF8.GetBytes("{\"CypherData\":\"s3KmjrLpFcvl\\u002BA==\",\"Nonce\":\"FwSyNO\\u002B52c1tpo9K\",\"Tag\":\"1ddCWDR6y3XfKti32vk6JA==\"}");
        private byte[] _tamperedCacheData = Encoding.UTF8.GetBytes("{\"CypherData\":\"\\u002BY3Rmac0Xvq8WeRz4ULSzxI=\",\"Nonce\":\"FwSyNO\\u002B52c1tpo9K\",\"Tag\":\"1ddCWDR6y3XfKti32vk6JA==\"}");

        public EncryptedDistributedCacheTests()
        {
            _baseCacheMock = new Mock<IDistributedCache>();
            _aesCryptoService = new AesCryptoService();
            _config = new Mock<IOptions<AesCryptoConfig>>();
            _config.Setup(m => m.Value).Returns(new AesCryptoConfig() { AesEncryptionKey = "keykeykeykeykeykeykeykeykeykeyke" });
            _encryptedDistributedCache = new EncryptedDistributedCache(_baseCacheMock.Object, _aesCryptoService, _config.Object);
        }


        [Fact]
        public async Task GetDataFromRedis_WithValidKey_ReturnsDecryptedData()
        {
            //Arrange
            _baseCacheMock.Setup(m => m.GetAsync(_key, default)).ReturnsAsync(_encryptedCacheData);

            //Act
            var result = await _encryptedDistributedCache.GetAsync(_key);

            // Assert
            Assert.Equal(_decryptedValue, result);
        }

        [Fact]
        public void GetDataFromRedis_WhenCypherDataIsInValid_ThrowsException()
        {
            //Arrange
            _baseCacheMock.Setup(m => m.GetAsync(_key, default)).ReturnsAsync(_tamperedCacheData);

            //Act
            var exception = Assert.ThrowsAsync<CryptographicException>(async () => await _encryptedDistributedCache.GetAsync(_key)).Result;

            // Assert
            Assert.Equal("The computed authentication tag did not match the input authentication tag.", exception.Message);
        }

        [Fact]
        public async Task GetDataFromRedis_WhenDataIsEmpty()
        {
            byte[] cachedValue = null;

            //Arrange
            _baseCacheMock.Setup(m => m.GetAsync(_key, default)).ReturnsAsync(cachedValue);

            //Act
            var result = await _encryptedDistributedCache.GetAsync(_key);

            // Assert
            Assert.Equal(cachedValue, result);
        }

        [Fact]
        public async Task SetDataInRedisAsync()
        {
            // Arrange
            _baseCacheMock.Setup(m => m.SetAsync(_key, It.IsAny<byte[]>(), _distributedCacheEntryOptions, default)).Verifiable();

            // Act
            await _encryptedDistributedCache.SetAsync(_key, _decryptedValue, _distributedCacheEntryOptions);

            // Assert
            _baseCacheMock.VerifyAll();
        }

        [Fact]
        public async Task SetDataInRedisAsync_WhenDataIsNull()
        {
            // Act
            await _encryptedDistributedCache.SetAsync(_key, null, _distributedCacheEntryOptions);

            // Assert
            _baseCacheMock.Verify(m => m.SetAsync(_key, null, _distributedCacheEntryOptions, default), Times.Once);
        }

        [Fact]
        public void GetSessionDataFromRedis()
        {
            //Act
            var exception = Assert.Throws<NotImplementedException>(() => _encryptedDistributedCache.Get(_key));
            //Assert
            Assert.IsType<NotImplementedException>(exception);
        }

        [Fact]
        public void RefreshDataInRedis()
        {
            //Act
            var exception = Assert.Throws<NotImplementedException>(() => _encryptedDistributedCache.Refresh(_key));
            //Assert
            Assert.IsType<NotImplementedException>(exception);
        }

        [Fact]
        public async Task RefreshDataInRedisAsync()
        {
            //Act
            await _encryptedDistributedCache.RefreshAsync(_key, default);
            //Assert
            _baseCacheMock.Verify(m => m.RefreshAsync(_key, default), Times.Once);
        }

        [Fact]
        public void RemoveDataInRedis()
        {
            //Act
            var exception = Assert.Throws<NotImplementedException>(() => _encryptedDistributedCache.Remove(_key));
            //Assert
            Assert.IsType<NotImplementedException>(exception);
        }

        [Fact]
        public async Task RemoveDataInRedisAsync()
        {
            //Act
            await _encryptedDistributedCache.RemoveAsync(_key, default);
            //Assert
            _baseCacheMock.Verify(m => m.RemoveAsync(_key, default), Times.Once);
        }

        [Fact]
        public void SetDataInRedis()
        {
            var value = Encoding.UTF8.GetBytes("some data");

            //Act
            var exception = Assert.Throws<NotImplementedException>(() => _encryptedDistributedCache.Set(_key, value, _distributedCacheEntryOptions));
            //Assert
            Assert.IsType<NotImplementedException>(exception);
        }
    }
}