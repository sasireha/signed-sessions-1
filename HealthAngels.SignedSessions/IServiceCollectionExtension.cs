using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.DependencyInjection;
using HealthAngels.SignedSessions.Session;
using HealthAngels.SignedSessions.Signature;

namespace HealthAngels.SignedSessions
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddSignedSessions(this IServiceCollection services, Action<SignatureSecrets> signatureSecrets)
        {
            services.Configure(signatureSecrets);
            
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
            services.AddSingleton<ISignedDistributedCache,SignedDistributedCache>();
            services.AddSingleton<ISignatureHelper, SignatureHelper>();
            services.AddTransient<ISessionStore, SignedSessionStore>();
        }
    }
}