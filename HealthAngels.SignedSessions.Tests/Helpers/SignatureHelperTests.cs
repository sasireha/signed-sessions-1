using System;
using Microsoft.Extensions.Options;
using Moq;
using HealthAngels.SignedSessions.Session;
using HealthAngels.SignedSessions.Signature;
using Xunit;

namespace HealthAngels.SignedSessions.Tests.Helpers
{
    public class SignatureHelperTests
    {
        const string secretKey = "89DF83B0-0337-467E-A111-B041472C4EC8";
        private ISignatureHelper signatureHelper;
        byte[] sessionValue = System.Text.Encoding.UTF8.GetBytes("testvalue");
        public SignatureHelperTests()
        {
            var sessionConfig = new SessionConfig { HmacSecretKey = secretKey };
            var sessionConfigOptions = new Mock<IOptions<SessionConfig>>();
            sessionConfigOptions.Setup(m => m.Value).Returns(sessionConfig);
            signatureHelper = new SignatureHelper(sessionConfigOptions.Object);
        }


        [Fact]
        public void CreateSignature_WithEmptySessionValue()
        {
            string signature = "";

            // Act
            signature = signatureHelper.CreateSignature(null);

            // Assert
            Assert.Empty(signature);
        }

        [Fact]
        public void CreateSignature_WithValue()
        {
            string signature = "";

            //Act
            signature = signatureHelper.CreateSignature(sessionValue);

            //Assert
            Assert.NotEmpty(signature);
        }

        [Fact]
        public void VerifySignature_WithCorrectValue()
        {            
            //Arrange
            string unsignedSessionData = Convert.ToBase64String(sessionValue);
            string signedSessionData = signatureHelper.CreateSignature(sessionValue);

            //Act
            bool result = signatureHelper.VerifySignature(unsignedSessionData, signedSessionData);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifySignature_WithTamperedValue()
        {
            //Arrange
            string tamperedSessionData = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("testvalue1"));
            string signedSessionData = signatureHelper.CreateSignature(sessionValue);
            string dataInRedis = tamperedSessionData + "." + signedSessionData;

            //Act
            bool result = signatureHelper.VerifySignature(tamperedSessionData, signedSessionData);

            //Assert
            Assert.False(result);
        }
    }
}