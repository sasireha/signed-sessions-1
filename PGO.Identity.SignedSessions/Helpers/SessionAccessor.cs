using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PGO.Identity.SignedSessions.Helpers
{
    public class SessionAccessor : ISessionAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private ISession Session => _httpContextAccessor.HttpContext.Session;

        public SessionAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<string> GetIdAsync(CancellationToken cancellationToken = default)
        {
            await Session.LoadAsync(cancellationToken);
            return Session.Id;
        }

        public async Task ClearAsync(CancellationToken cancellationToken = default)
        {
            await Session.LoadAsync(cancellationToken);
            Session.Clear();
        }

        public async Task<IEnumerable<string>> GetKeysAsync(CancellationToken cancellationToken = default)
        {
            await Session.LoadAsync(cancellationToken);
            return Session.Keys;
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await Session.CommitAsync(cancellationToken);
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            await Session.LoadAsync(cancellationToken);
            Session.Remove(key);
        }

        public async Task SetAsync(string key, byte[] value, CancellationToken cancellationToken = default)
        {
            await Session.LoadAsync(cancellationToken);
            Session.Set(key, value);
        }

        public async Task SetStringAsync(string key, string value, CancellationToken cancellationToken = default)
        {
            await Session.LoadAsync(cancellationToken);
            Session.SetString(key, value);
        }

        public async Task<byte[]> GetAsync(string key, CancellationToken cancellationToken = default)
        {
            await Session.LoadAsync(cancellationToken);
            return Session.Get(key);
        }

        public async Task<string> GetStringAsync(string key, CancellationToken cancellationToken = default)
        {
            await Session.LoadAsync(cancellationToken);
            return Session.GetString(key);
        }
    }
}