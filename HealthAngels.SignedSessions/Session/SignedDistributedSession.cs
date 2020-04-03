using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace HealthAngels.SignedSessions.Session
{
    //The default session provider in ASP.NET Core loads session records synchronously from the underlying IDistributedCache backing store. Inorder to load asynchronously the ISession.LoadAsync method should be explicitly called.
    //This class enforces this pattern. reference: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-3.1
    public class SignedDistributedSession : DistributedSession, ISession
    {
        public SignedDistributedSession(IDistributedCache cache, string sessionKey, TimeSpan idleTimeout, TimeSpan ioTimeout,
            Func<bool> tryEstablishSession, ILoggerFactory loggerFactory, bool isNewSessionKey) : base(cache, sessionKey, idleTimeout, ioTimeout, tryEstablishSession, loggerFactory, isNewSessionKey)
        {

        }

        private bool IsLoadedAsync { get; set; } = false;
        public new void Clear()
        {
            base.Clear();
        }
        public new Task CommitAsync(CancellationToken cancellationToken = default)
        {
            return base.CommitAsync(cancellationToken);
        }
        public new Task LoadAsync(CancellationToken cancellationToken = default)
        {
            IsLoadedAsync = true;
            return base.LoadAsync(cancellationToken);
        }
        public new void Remove(string key)
        {
            if (IsLoadedAsync)
            {
                base.Remove(key);
            }
            else
            {
                throw new Exception("Session was not loaded asynchronously");
            }
        }
        public new void Set(string key, byte[] value)
        {
            if (IsLoadedAsync)
            {
                base.Set(key, value);
            }
            else
            {
                throw new Exception("Session was not loaded asynchronously");
            }
        }
        public new bool TryGetValue(string key, out byte[] value)
        {
            if (IsLoadedAsync)
            {
                return base.TryGetValue(key, out value);
            }
            else
            {
                throw new Exception("Session was not loaded asynchronously");
            }
        }
    }
}