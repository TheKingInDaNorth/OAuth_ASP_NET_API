using System;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IdentityServer3.Core.Configuration;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SocialNetwork.OAuth.Startup))]

namespace SocialNetwork.OAuth
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var inMemoryManager = new InMemoryManager();
            var factory = new IdentityServerServiceFactory()
                .UseInMemoryUsers(inMemoryManager.GetUsers())
                .UseInMemoryScopes(inMemoryManager.GetScopes())
                .UseInMemoryClients(inMemoryManager.GetClients());
            var certificate = Convert.FromBase64String(ConfigurationManager.AppSettings["SigningCertificate"]);
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            var options = new IdentityServerOptions
            { 
                SigningCertificate = new X509Certificate2(certificate, ConfigurationManager.AppSettings["SigningCertificatePassword"]),
                RequireSsl = false, //DO NOT DO THIS IN PRODUCTION
                Factory = factory
            };
            app.UseIdentityServer(options);
        }
    }
}
