using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;

namespace PGO.Identity.SignedSessions.Helpers
{
      /// <summary>
    /// Class for accessing the HTTP <see cref="ISession"/> in other classes besides the <see cref="ControllerBase"/>.
    /// It also makes sure that all actions on the session are done async.
    /// See the source code of the underlying <see cref="DistributedSession"/> https://github.com/dotnet/aspnetcore/blob/master/src/Middleware/Session/src/DistributedSession.cs
    /// where the synchronous Load() method is called. This call should be prevented by calling LoadAsync before.
    /// </summary>
    public interface ISessionAccessor
    {
        /// <summary>
        /// A unique identifier for the current session. This is not the same as the session
        /// cookie since the cookie lifetime may not be the same as the session entry lifetime
        /// in the data store.
        /// </summary>
        Task<string> GetIdAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Remove all entries from the current session, if any. The session cookie is not removed.
        /// </summary>
        Task ClearAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Enumerates all the keys, if any.
        /// </summary>
        Task<IEnumerable<string>> GetKeysAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Store the session in the data store. This may throw if the data store is unavailable.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task CommitAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Remove the given key from the session if present.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task RemoveAsync(string key, CancellationToken cancellationToken = default);

        /// <summary>
        /// Set the given key and value in the current session. This will throw if the session was not established prior to sending the response.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetAsync(string key, byte[] value, CancellationToken cancellationToken = default);

        Task SetStringAsync(string key, string value, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve the value of the given key, if present.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<byte[]> GetAsync(string key, CancellationToken cancellationToken = default);

        Task<string> GetStringAsync(string key, CancellationToken cancellationToken = default);
    }
}