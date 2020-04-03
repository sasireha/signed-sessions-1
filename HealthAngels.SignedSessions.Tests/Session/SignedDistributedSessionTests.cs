using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using HealthAngels.SignedSessions.Session;
using Xunit;

namespace HealthAngels.SignedSessions.Tests.Session
{
    public class SignedDistributedSessionTests
    {        
        private SignedDistributedSession _signedDistributedSession;
        private Mock<IDistributedCache> _cacheMock;
        private Mock<ILoggerFactory> _logger;
        private string sessionKey = Guid.NewGuid().ToString();
        private TimeSpan idleTimeout = new TimeSpan(1200);
        private TimeSpan ioTimeout = new TimeSpan(1200);
        private Func<bool> tryEstablishSession = () => true;
        private bool isNewSessionKey = true;
        public SignedDistributedSessionTests()
        {
            _logger = new Mock<ILoggerFactory>();
            _cacheMock = new Mock<IDistributedCache>();
            _signedDistributedSession = new SignedDistributedSession(_cacheMock.Object, sessionKey, idleTimeout, ioTimeout, tryEstablishSession, _logger.Object, isNewSessionKey);
        }


        [Fact]
        public void Remove_WhenSessionIsNotLoadedAsynchronously_ThrowsException()
        {
            //Act
            var exception = Assert.Throws<Exception>(() => _signedDistributedSession.Remove(sessionKey));
            //Assert
            Assert.Equal("Session was not loaded asynchronously", exception.Message);
        }

        [Fact]
        public void Set_WhenSessionIsNotLoadedAsynchronously_ThrowsException()
        {
            byte[] value = System.Text.Encoding.UTF8.GetBytes("testvalue");
            //Act
            var exception = Assert.Throws<Exception>(() => _signedDistributedSession.Set(sessionKey, value));
            //Assert
            Assert.Equal("Session was not loaded asynchronously", exception.Message);
        }

        [Fact]
        public void TryGetValue_WhenSessionIsNotLoadedAsynchronously_ThrowsException()
        {
            //Act
            var exception = Assert.Throws<Exception>(() => _signedDistributedSession.TryGetValue(sessionKey, out byte[] value));
            //Assert
            Assert.Equal("Session was not loaded asynchronously", exception.Message);
        }

        [Fact]
        public void TryGetValue_WhenSessionIsLoadAsynchronously_ReturnsValueStoredInSession()
        {
            byte[] value = System.Text.Encoding.UTF8.GetBytes("testvalue");
            //Arrange
            _signedDistributedSession.LoadAsync(default);
            _signedDistributedSession.Set(sessionKey, value);

            //Act
            _signedDistributedSession.TryGetValue(sessionKey, out byte[] result);

            //Assert
            Assert.Equal(value, result);
        }
    }
}