using System;
using HealthAngels.SignedSessions.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using HealthAngels.SignedSessions.Session;
using Xunit;

namespace HealthAngels.SignedSessions.Tests.Session
{
    public class SignedSessionStoreTests
    {
        private readonly SignedSessionStore _signedSessionStore;

        public SignedSessionStoreTests()
        {
            var mockSignedDistributedCache = new Mock<ISignedDistributedCache>();
            var mockLoggerFactory = new Mock<ILoggerFactory>();
            _signedSessionStore = new SignedSessionStore(mockSignedDistributedCache.Object, mockLoggerFactory.Object);
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
            ISession result = _signedSessionStore.Create(sessionKey, idleTimeout, ioTimeout, tryEstablishSession, isNewSessionKey);

            //Assert
            Assert.IsType<SignedDistributedSession>(result);
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
            var exception = Assert.Throws<ArgumentNullException>(() => _signedSessionStore.Create(sessionKey, idleTimeout, ioTimeout, tryEstablishSession, isNewSessionKey));

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
            var exception = Assert.Throws<ArgumentNullException>(() => _signedSessionStore.Create(sessionKey, idleTimeout, ioTimeout, tryEstablishSession, isNewSessionKey));

            //Assert
            Assert.IsType<ArgumentNullException>(exception);
        }
    }
}