using System.Text;

namespace HealthAngels.SignedSessions.Signature
{
    public interface ISignatureHelper
    {
        Encoding Encoding { get; }
        string CreateSignature(byte[] message);
        bool VerifySignature(string unsignedValue, string signature);
    }
}