using HealthAngels.EncryptedSessions.Session;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace HealthAngels.EncryptedSessions.Tests.Session
{
    public class EncryptedDistributedSessionTests
    {        
        private EncryptedDistributedSession _encryptedDistributedSession;
        private Mock<IDistributedCache> _cacheMock;
        private Mock<ILoggerFactory> _logger;
        private string sessionKey = Guid.NewGuid().ToString();
        private TimeSpan idleTimeout = new TimeSpan(1200);
        private TimeSpan ioTimeout = new TimeSpan(1200);
        private Func<bool> tryEstablishSession = () => true;
        private bool isNewSessionKey = true;
        public EncryptedDistributedSessionTests()
        {
            _logger = new Mock<ILoggerFactory>();
            _cacheMock = new Mock<IDistributedCache>();
            _encryptedDistributedSession = new EncryptedDistributedSession(_cacheMock.Object, sessionKey, idleTimeout, ioTimeout, tryEstablishSession, _logger.Object, isNewSessionKey);
        }


        [Fact]
        public void Remove_WhenSessionIsNotLoadedAsynchronously_ThrowsException()
        {
            //Act
            var exception = Assert.Throws<Exception>(() => _encryptedDistributedSession.Remove(sessionKey));
            //Assert
            Assert.Equal("Session was not loaded asynchronously", exception.Message);
        }

        [Fact]
        public void Set_WhenSessionIsNotLoadedAsynchronously_ThrowsException()
        {
            byte[] value = System.Text.Encoding.UTF8.GetBytes("testvalue");
            //Act
            var exception = Assert.Throws<Exception>(() => _encryptedDistributedSession.Set(sessionKey, value));
            //Assert
            Assert.Equal("Session was not loaded asynchronously", exception.Message);
        }

        [Fact]
        public void TryGetValue_WhenSessionIsNotLoadedAsynchronously_ThrowsException()
        {
            //Act
            var exception = Assert.Throws<Exception>(() => _encryptedDistributedSession.TryGetValue(sessionKey, out byte[] value));
            //Assert
            Assert.Equal("Session was not loaded asynchronously", exception.Message);
        }

        [Fact]
        public void TryGetValue_WhenSessionIsLoadAsynchronously_ReturnsValueStoredInSession()
        {
            byte[] value = System.Text.Encoding.UTF8.GetBytes("testvalue");
            //Arrange
            _encryptedDistributedSession.LoadAsync(default);
            _encryptedDistributedSession.Set(sessionKey, value);

            //Act
            _encryptedDistributedSession.TryGetValue(sessionKey, out byte[] result);

            //Assert
            Assert.Equal(value, result);
        }
    }
}