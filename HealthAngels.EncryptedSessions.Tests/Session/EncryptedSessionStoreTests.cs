using HealthAngels.EncryptedSessions.Cache;
using HealthAngels.EncryptedSessions.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace HealthAngels.EncryptedSessions.Tests.Session
{
    public class EncryptedSessionStoreTests
    {
        private readonly EncryptedSessionStore _encryptedSessionStore;

        public EncryptedSessionStoreTests()
        {
            var mockEncryptedDistributedCache = new Mock<IEncryptedDistributedCache>();
            var mockLoggerFactory = new Mock<ILoggerFactory>();
            _encryptedSessionStore = new EncryptedSessionStore(mockEncryptedDistributedCache.Object, mockLoggerFactory.Object);
        }

        [Fact]
        public void CreatesDistributedSession_WithCorrectParameters()
        {
            //Arrange
            string sessionKey = Guid.NewGuid().ToString();
            TimeSpan idleTimeout = new TimeSpan();
            TimeSpan ioTimeout = new TimeSpan();
            Func<bool> tryEstablishSession = () => true;
            bool isNewSessionKey = true;

            //Act
            ISession result = _encryptedSessionStore.Create(sessionKey, idleTimeout, ioTimeout, tryEstablishSession, isNewSessionKey);

            //Assert
            Assert.IsType<EncryptedDistributedSession>(result);
        }

        [Fact]
        public void CreatesDistributedSession_WithNullSessionId()
        {
            //Arrange
            string sessionKey = null;
            TimeSpan idleTimeout = new TimeSpan();
            TimeSpan ioTimeout = new TimeSpan();
            Func<bool> tryEstablishSession = () => true;
            bool isNewSessionKey = true;

            //Act
            var exception = Assert.Throws<ArgumentNullException>(() => _encryptedSessionStore.Create(sessionKey, idleTimeout, ioTimeout, tryEstablishSession, isNewSessionKey));

            //Assert
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void CreatesDistributedSession_WithNullTryEstablishSessionArgument()
        {
            //Arrange
            string sessionKey = Guid.NewGuid().ToString(); 
            TimeSpan idleTimeout = new TimeSpan();
            TimeSpan ioTimeout = new TimeSpan();
            Func<bool> tryEstablishSession = null;
            bool isNewSessionKey = true;

            //Act
            var exception = Assert.Throws<ArgumentNullException>(() => _encryptedSessionStore.Create(sessionKey, idleTimeout, ioTimeout, tryEstablishSession, isNewSessionKey));

            //Assert
            Assert.IsType<ArgumentNullException>(exception);
        }
    }
}