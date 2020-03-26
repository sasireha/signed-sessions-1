using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using PGO.Identity.SignedSessions.Helpers;
using Xunit;

namespace PGO.Identity.SignedSessions.Tests.Helpers
{
    public class SessionAccessorTests
    {
        private readonly Mock<ISession> _sessionMock;

        private readonly SessionAccessor _sessionAccessor;

        public SessionAccessorTests()
        {
            _sessionMock = new Mock<ISession>();

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(m => m.HttpContext)
                .Returns(new DefaultHttpContext() { Session = _sessionMock.Object }); ;

            _sessionAccessor = new SessionAccessor(httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task GetIdAsync_ShouldReturn_SessionId()
        {
            const string id = "Calimero";

            // Arrange
            _sessionMock.SetupGet(m => m.Id).Returns(id);

            // Act
            var result = await _sessionAccessor.GetIdAsync();


            // Act & Assert
            Assert.Equal(id, result);
            _sessionMock.Verify(m => m.LoadAsync(default), Times.Once);
        }

        [Fact]
        public async Task ClearAsync_ShouldCall_SessionLoadAndClear()
        {
            // Act
            await _sessionAccessor.ClearAsync();

            // Assert
            _sessionMock.Verify(m => m.LoadAsync(default), Times.Once);
            _sessionMock.Verify(m => m.Clear(), Times.Once);
        }

        [Fact]
        public async Task GetKeysAsync_ShouldReturn_SessionKeys()
        {
            string[] keys = { "A", "be", "ccc" };

            // Arrange
            _sessionMock.SetupGet(m => m.Keys).Returns(keys);

            // Act
            var result = await _sessionAccessor.GetKeysAsync();
            
            // Assert
            Assert.Equal(keys, result);
            _sessionMock.Verify(m => m.LoadAsync(default), Times.Once);
        }

        [Fact]
        public async Task CommitAsync_ShouldCall_SessionCommitAsync()
        {
            // Act
            await _sessionAccessor.CommitAsync(default);

            // Assert
            _sessionMock.Verify(m => m.CommitAsync(default), Times.Once);
        }

        [Fact]
        public async Task RemoveAsync_ShouldCall_SessionRemoveAsync()
        {
            // Act
            await _sessionAccessor.RemoveAsync(default);

            // Assert
            _sessionMock.Verify(m => m.LoadAsync(default), Times.Once);
            _sessionMock.Verify(m => m.Remove(default), Times.Once);
        }

        [Fact]
        public async Task SetAsync_ShouldSetValue_InSession()
        {
            const string key = "Milk";
            byte[] value = { 1, 2, 3 };

            // Act
            await _sessionAccessor.SetAsync(key, value);

            // Assert
            _sessionMock.Verify(m => m.LoadAsync(default), Times.Once);
            _sessionMock.Verify(m => m.Set(key, value), Times.Once);
        }

        [Fact]
        public async Task SetStringAsync_ShouldSetValue_InSession()
        {
            const string key = "Milk";
            const string value = "Honey";

            // Act
            await _sessionAccessor.SetStringAsync(key, value);

            // Assert
            _sessionMock.Verify(m => m.LoadAsync(default), Times.Once);
            _sessionMock.Verify(m => m.Set(key, It.Is<byte[]>(s => s.SequenceEqual(Encoding.UTF8.GetBytes(value)))), Times.Once);
        }

        [Fact]
        public async Task GetAsync_ShouldGetValue_FromSession()
        {
            const string key = "Milk";
            byte[] value = { 1, 2, 3 };

            // Arrange
            _sessionMock.Setup(m => m.TryGetValue(key, out value)).Returns(true);

            // Act
            var result= await _sessionAccessor.GetAsync(key);

            // Assert
            Assert.Equal(value, result);
            _sessionMock.Verify(m => m.LoadAsync(default), Times.Once);
        }

        [Fact]
        public async Task GetStringAsync_ShouldGetValue_FromSession()
        {
            const string key = "Milk";
            const string value = "Honey";
            var byteValue = Encoding.UTF8.GetBytes(value);

            // Arrange
            _sessionMock.Setup(m => m.TryGetValue(key, out byteValue)).Returns(true);

            // Act
            var result = await _sessionAccessor.GetStringAsync(key);

            // Assert
            Assert.Equal(value, result);
            _sessionMock.Verify(m => m.LoadAsync(default), Times.Once);
        }
    }
}