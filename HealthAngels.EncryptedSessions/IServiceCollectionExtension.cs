using HealthAngels.EncryptedSessions.AesCrypto;
using HealthAngels.EncryptedSessions.Cache;
using HealthAngels.EncryptedSessions.Session;
using HealthAngels.EncryptedSessions.Signature;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.DependencyInjection;

namespace HealthAngels.EncryptedSessions
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddEncryptedSessions(this IServiceCollection services)
        {            
            ConfigureDependencies(services);
        
            services.AddSession(options =>
            {
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.HttpOnly = true;
                /* SameSite must be set to Lax:
                 * https://tools.ietf.org/html/draft-ietf-httpbis-rfc6265bis-03#section-5.3.7.1
                 * In Lax mode, the browser will still send the cookie when a cross-domain “top level” GET request is triggered. This is necessary because Strict mode
                 * breaks some functionality across the web. For example, if your session value for a given site is set with SameSite=Strict; when you click a link to
                 * go to that site from a different site, you’ll arrive without a session.
                */
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.IsEssential = true;
            });
            
            return services;
        }

        private static void ConfigureDependencies(IServiceCollection services)
        {
            services.AddSingleton<IEncryptedDistributedCache,EncryptedDistributedCache>();
            services.AddSingleton<ISignatureHelper, SignatureHelper>();
            services.AddTransient<ISessionStore, EncryptedSessionStore>();
            services.AddTransient<IAesCryptoService, AesCryptoService>();
        }
    }
}