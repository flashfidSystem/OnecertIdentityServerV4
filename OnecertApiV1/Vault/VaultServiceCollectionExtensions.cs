using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace OnecertApiV1.Vault
{
    public static class VaultServiceCollectionExtensions
    {
        public static IServiceCollection AddHashiCorpVault(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<VaultOptions>(configuration.GetSection(VaultOptions.SectionName));
            services.TryAddSingleton<VaultClientCertificateProvider>();

            services.AddHttpClient(HashiCorpVaultService.AuthClientName)
                .ConfigurePrimaryHttpMessageHandler(sp =>
                {
                    var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger("VaultAuthClient");
                    logger.LogInformation("[VaultAuthClientHandler] Creating handler...");
                    var certificateProvider = sp.GetRequiredService<VaultClientCertificateProvider>();

                    var handler = new SocketsHttpHandler();

                    var certificate = certificateProvider.Certificate;
                    if (certificate is not null)
                    {
                        logger.LogInformation("[VaultAuthClientHandler] Attaching client certificate: Subject={Subject}, HasPrivateKey={HasKey}, Thumbprint={Thumb}",
                            certificate.Subject, certificate.HasPrivateKey, certificate.Thumbprint);
                        handler.SslOptions.ClientCertificates = new X509CertificateCollection { certificate };
                    }
                    else
                    {
                        logger.LogWarning("[VaultAuthClientHandler] No client certificate available.");
                    }

                    logger.LogInformation("[VaultAuthClientHandler] Handler created with SocketsHttpHandler (OS-default SSL).");
                    return handler;
                });

            services.AddHttpClient(HashiCorpVaultService.ApiClientName)
                .ConfigurePrimaryHttpMessageHandler(sp =>
                {
                    var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger("VaultApiClient");
                    logger.LogInformation("[VaultApiClientHandler] Creating handler (no client cert)...");
                    return new SocketsHttpHandler();
                });

            services.TryAddSingleton<IHashiCorpVaultService, HashiCorpVaultService>();
            services.AddHostedService<VaultCredentialRefreshHostedService>();

            return services;
        }
    }
}
