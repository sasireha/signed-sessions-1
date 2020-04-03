using System;
using System.Text;
using System.Threading.Tasks;
using HealthAngels.SignedSessions.Cache;
using HealthAngels.SignedSessions.Helpers;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Xunit;

namespace HealthAngels.SignedSessions.Tests.Session
{
     public class SignedDistributedCacheTests
    {
        private readonly Mock<IDistributedCache> _baseCacheMock;
        private readonly Mock<ISignatureHelper> _signatureHelper;

        private readonly SignedDistributedCache _signedDistributedCache;
        private string _key = Guid.NewGuid().ToString();
        private const string UnsignedValue = "dGVzdHZhbHVl"; // must be base64 encoded
        private const string Signature = "tCSsoVusDeFis9E5QEtDXSsoOrnqiPQeheDXkkVkQv0="; // must be base64 encoded
        private DistributedCacheEntryOptions _distributedCacheEntryOptions = new DistributedCacheEntryOptions();
        private byte[] _cachedValue = Encoding.UTF8.GetBytes("test session data");
        private byte[] _signedData = Encoding.UTF8.GetBytes(UnsignedValue + "." + Signature);

        public SignedDistributedCacheTests()
        {
            _baseCacheMock = new Mock<IDistributedCache>();
            _signatureHelper = new Mock<ISignatureHelper>();
            _signedDistributedCache = new SignedDistributedCache(_baseCacheMock.Object, _signatureHelper.Object);
        }

        [Fact]
        public async Task GetDataFromRedis_WhenSignatureIsValid_ReturnsData()
        {
            //Arrange
            _baseCacheMock.Setup(m => m.GetAsync(_key, default)).ReturnsAsync(_signedData);
            _signatureHelper.Setup(m => m.VerifySignature(UnsignedValue, Signature)).Returns(true);

            //Act
            var result = await _signedDistributedCache.GetAsync(_key);

            // Assert
            Assert.Equal(Convert.FromBase64String(UnsignedValue), result);
        }

        [Fact]
        public void GetDataFromRedis_WhenSignatureIsInValid_ThrowsException()
        {   
            //Arrange
            _baseCacheMock.Setup(m => m.GetAsync(_key, default)).ReturnsAsync(_signedData);
            _signatureHelper.Setup(m => m.VerifySignature(UnsignedValue, Signature)).Returns(false);

            //Act
            Exception exception = Assert.ThrowsAsync<Exception>(async () => await _signedDistributedCache.GetAsync(_key)).Result;

            // Assert
            Assert.Equal("Session Signature is invalid", exception.Message);
        }

        [Fact]
        public async Task GetDataFromRedis_WhenDataIsEmpty()
        {
            _cachedValue = null;

            //Arrange
            _baseCacheMock.Setup(m => m.GetAsync(_key, default)).ReturnsAsync(_cachedValue);

            //Act
            var result = await _signedDistributedCache.GetAsync(_key);

            // Assert
            Assert.Equal(_cachedValue, result);
        }

        [Fact]
        public async Task SetDataInRedisAsync()
        {            
            var sessionDataWithSignature = Encoding.UTF8.GetBytes(Convert.ToBase64String(_cachedValue) + "." + Signature);
            // Arrange
            _signatureHelper.Setup(m => m.CreateSignature(_cachedValue)).Returns(Signature);

            // Act
            await _signedDistributedCache.SetAsync(_key, _cachedValue, _distributedCacheEntryOptions);

            // Assert
            _baseCacheMock.Verify(m => m.SetAsync(_key, sessionDataWithSignature, _distributedCacheEntryOptions, default), Times.Once);
        }

        [Fact]
        public async Task SetDataInRedisAsync_WhenDataIsNull()
        {
            // Act
            await _signedDistributedCache.SetAsync(_key, null, _distributedCacheEntryOptions);

            // Assert
            _baseCacheMock.Verify(m => m.SetAsync(_key, null, _distributedCacheEntryOptions, default), Times.Once);
        }

        [Fact]
        public void GetSessionDataFromRedis()
        {
            //Act
            var exception = Assert.Throws<NotImplementedException>(() => _signedDistributedCache.Get(_key));
            //Assert
            Assert.IsType<NotImplementedException>(exception);
        }

        [Fact]
        public void RefreshDataInRedis()
        {
            //Act
            var exception = Assert.Throws<NotImplementedException>(() => _signedDistributedCache.Refresh(_key));
            //Assert
            Assert.IsType<NotImplementedException>(exception);
        }

        [Fact]
        public async Task RefreshDataInRedisAsync()
        {
            //Act
            await _signedDistributedCache.RefreshAsync(_key, default);
            //Assert
            _baseCacheMock.Verify(m => m.RefreshAsync(_key, default), Times.Once);
        }

        [Fact]
        public void RemoveDataInRedis()
        {
            //Act
            var exception = Assert.Throws<NotImplementedException>(() => _signedDistributedCache.Remove(_key));
            //Assert
            Assert.IsType<NotImplementedException>(exception);
        }

        [Fact]
        public async Task RemoveDataInRedisAsync()
        {
            //Act
            await _signedDistributedCache.RemoveAsync(_key, default);
            //Assert
            _baseCacheMock.Verify(m => m.RemoveAsync(_key, default), Times.Once);
        }

        [Fact]
        public void SetDataInRedis()
        {
            //Act
            var exception = Assert.Throws<NotImplementedException>(() => _signedDistributedCache.Set(_key, _cachedValue, _distributedCacheEntryOptions));
            //Assert
            Assert.IsType<NotImplementedException>(exception);
        }
    }
}