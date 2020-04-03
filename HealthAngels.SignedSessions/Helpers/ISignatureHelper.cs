using System.Text;

namespace HealthAngels.SignedSessions.Helpers
{
    public interface ISignatureHelper
    {
        Encoding Encoding { get; }
        string CreateSignature(byte[] message);
        bool VerifySignature(string unsignedValue, string signature);
    }
}