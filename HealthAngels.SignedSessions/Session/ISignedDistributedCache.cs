using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAngels.SignedSessions.Session
{
    public interface ISignedDistributedCache:IDistributedCache
    {
    }
}
