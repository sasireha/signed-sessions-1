using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using HealthAngels.SignedSessions.Session;
using Xunit;

namespace HealthAngels.SignedSessions.Tests.Session
{
    public class CustomSessionStoreTests
    {
        private readonly CustomSessionStore _customSessionStore;

        public CustomSessionStoreTests()
        {
            var mockCustomRedisCache = new Mock<CustomRedisCache>();
            var mockLoggerFactory = new Mock<ILoggerFactory>();
            _customSessionStore = new CustomSessionStore(mockCustomRedisCache.Object, mockLoggerFactory.Object);
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
            ISession result = _customSessionStore.Create(sessionKey, idleTimeout, ioTimeout, tryEstablishSession, isNewSessionKey);

            //Assert
            Assert.IsType<CustomDistributedSession>(result);
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
            var exception = Assert.Throws<ArgumentNullException>(() => _customSessionStore.Create(sessionKey, idleTimeout, ioTimeout, tryEstablishSession, isNewSessionKey));

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
            var exception = Assert.Throws<ArgumentNullException>(() => _customSessionStore.Create(sessionKey, idleTimeout, ioTimeout, tryEstablishSession, isNewSessionKey));

            //Assert
            Assert.IsType<ArgumentNullException>(exception);
        }
    }
}