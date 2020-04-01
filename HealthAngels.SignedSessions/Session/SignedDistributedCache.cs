using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using HealthAngels.SignedSessions.Signature;

namespace HealthAngels.SignedSessions.Session
{
    public class SignedDistributedCache : ISignedDistributedCache
    {
        private readonly IDistributedCache _cache;
        private readonly ISignatureHelper _signatureHelper;

        public SignedDistributedCache()
        { }
        public SignedDistributedCache(IDistributedCache cache, ISignatureHelper signatureHelper)
        {
            _cache = cache;
            _signatureHelper = signatureHelper;
        }

        public async Task<byte[]> GetAsync(string key, CancellationToken token = default)
        {
            var sessionData = await _cache.GetAsync(key, token);
            if (sessionData == null || sessionData.Length == 0)
            {
                return sessionData;
            }
          
            var signedValue = Encoding.UTF8.GetString(sessionData).Split(".");
            var encodedValue = signedValue.First();
            var signature = signedValue.Last();
            
            var signatureIsValid = _signatureHelper.VerifySignature(encodedValue, signature);

            if (!signatureIsValid)
            {
                throw new Exception("Session Signature is invalid");
            }
            
            return Convert.FromBase64String(encodedValue);
        }

        public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            if (value == null || value.Length == 0)
            {
                await _cache.SetAsync(key, value, options, token);
                return;
            }
            
            var signature = _signatureHelper.CreateSignature(value);
            var encodedActualValue = Convert.ToBase64String(value);
            var signedValue = Encoding.UTF8.GetBytes(encodedActualValue + "." + signature);
            await _cache.SetAsync(key, signedValue, options, token);
        }

        public byte[] Get(string key)
        {
            throw new NotImplementedException();
        }

        public void Refresh(string key)
        {
            throw new NotImplementedException();
        }
        public Task RefreshAsync(string key, CancellationToken token = default)
        {
            return _cache.RefreshAsync(key, token);
        }

        public void Remove(string key)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(string key, CancellationToken token = default)
        {
            return _cache.RemoveAsync(key, token);
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            throw new NotImplementedException();
        }
    }
}