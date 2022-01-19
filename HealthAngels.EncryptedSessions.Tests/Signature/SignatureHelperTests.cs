using HealthAngels.EncryptedSessions.Signature;
using Microsoft.Extensions.Options;
using Moq;
using System;
using Xunit;

namespace HealthAngels.EncryptedSessions.Tests.Signature
{
    public class SignatureHelperTests
    {
        const string secretKey = "89DF83B0-0337-467E-A111-B041472C4EC8";
        private ISignatureHelper signatureHelper;
        byte[] actualValue = System.Text.Encoding.UTF8.GetBytes("testvalue");
        public SignatureHelperTests()
        {
            var signatureSecrets = new SignatureSecrets { HmacSecretKey = secretKey };
            var signatureSecretsOptions = new Mock<IOptions<SignatureSecrets>>();
            signatureSecretsOptions.Setup(m => m.Value).Returns(signatureSecrets);
            signatureHelper = new SignatureHelper(signatureSecretsOptions.Object);
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
            signature = signatureHelper.CreateSignature(actualValue);

            //Assert
            Assert.NotEmpty(signature);
        }

        [Fact]
        public void VerifySignature_WithCorrectValue()
        {            
            //Arrange
            string unsignedEncodedValue = Convert.ToBase64String(actualValue);
            string signature = signatureHelper.CreateSignature(actualValue);

            //Act
            bool result = signatureHelper.VerifySignature(unsignedEncodedValue, signature);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifySignature_WithTamperedValue()
        {
            //Arrange
            string tamperedValue = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("testvalue1"));
            string signature = signatureHelper.CreateSignature(actualValue);

            //Act
            bool result = signatureHelper.VerifySignature(tamperedValue, signature);

            //Assert
            Assert.False(result);
        }
    }
}