using HealthAngels.EncryptedSessions.AesCrypto;
using HealthAngels.EncryptedSessions.Signature;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace HealthAngels.EncryptedSessions.Cache
{
    public class EncryptedDistributedCache : IEncryptedDistributedCache
    {
        private readonly IDistributedCache _cache;
        private readonly ISignatureHelper _signatureHelper;
        private readonly IAesCryptoService _aesCryptoService;
        private readonly AesCryptoConfig _aesKeysConfig;

        public EncryptedDistributedCache(IDistributedCache cache, ISignatureHelper signatureHelper, IAesCryptoService aesCryptoService, IOptions<AesCryptoConfig> config)
        {
            _cache = cache;
            _signatureHelper = signatureHelper;
            _aesCryptoService = aesCryptoService;
            _aesKeysConfig = config.Value;
        }

        public async Task<byte[]> GetAsync(string key, CancellationToken token = default)
        {
            var value = await _cache.GetStringAsync(key, token);
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            if (value.Contains("."))
            {
                var signedValue = value.Split(".");
                var unsignedEncodedValue = signedValue.First();
                var signature = signedValue.Last();

                var signatureIsValid = _signatureHelper.VerifySignature(unsignedEncodedValue, signature);

                if (!signatureIsValid)
                {
                    throw new Exception("Session Signature is invalid");
                }

                return Convert.FromBase64String(unsignedEncodedValue);
            }
            else
            {
                var deserializedString = JsonSerializer.Deserialize<AesCryptoData>(value);
                var cypherData = Convert.FromBase64String(deserializedString.CypherData);
                var nonce = Convert.FromBase64String(deserializedString.Nonce);
                var tag = Convert.FromBase64String(deserializedString.Tag);
                return _aesCryptoService.DecryptAESGCM(cypherData, Encoding.UTF8.GetBytes(_aesKeysConfig.AesEncryptionKey), nonce, tag);
            }
        }

        public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            if (value == null || value.Length == 0)
            {
                await _cache.SetAsync(key, value, options, token);
                return;
            }

            var nonce = new byte[12]; // must be 12 bytes
            var tagBytes = new byte[16];
            RandomNumberGenerator.Fill(nonce);
            var cypherText = _aesCryptoService.EncryptAESGCM(value, Encoding.UTF8.GetBytes(_aesKeysConfig.AesEncryptionKey), nonce, tagBytes);
            var encryptedValue = _aesCryptoService.GetAesCryptoData(cypherText, nonce, tagBytes);
            string serializedValue = JsonSerializer.Serialize(encryptedValue);
            await _cache.SetStringAsync(key, serializedValue, options, token);
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