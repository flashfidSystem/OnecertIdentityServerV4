using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace OnecertApiV1.Vault
{
    public sealed class VaultClientCertificateProvider : IDisposable
    {
        private readonly Lazy<X509Certificate2?> _certificate;
        private readonly ILogger<VaultClientCertificateProvider> _logger;

        public VaultClientCertificateProvider(IOptions<VaultOptions> options, ILogger<VaultClientCertificateProvider> logger)
        {
            _logger = logger;
            _certificate = new Lazy<X509Certificate2?>(() => LoadCertificate(options.Value));
        }

        public X509Certificate2? Certificate => _certificate.Value;

        private X509Certificate2? LoadCertificate(VaultOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.ClientCertificatePath))
            {
                _logger.LogInformation("[VaultCertProvider] No ClientCertificatePath configured.");
                return null;
            }

            if (!File.Exists(options.ClientCertificatePath))
            {
                throw new FileNotFoundException(
                    $"Vault client certificate not found at '{options.ClientCertificatePath}'. Check Vault:ClientCertificatePath.",
                    options.ClientCertificatePath);
            }
_logger.LogInformation("[VaultCertProvider] Loading certificate from {Path}", options.ClientCertificatePath);

var flags = X509KeyStorageFlags.MachineKeySet
            | X509KeyStorageFlags.PersistKeySet
            | X509KeyStorageFlags.Exportable;

var cert = new X509Certificate2(options.ClientCertificatePath, options.ClientCertificatePassword, flags);

_logger.LogInformation("[VaultCertProvider] Certificate loaded. Subject={Subject}, HasPrivateKey={HasKey}, Thumbprint={Thumb}, KeyAlgorithm={Algo}",
    cert.Subject, cert.HasPrivateKey, cert.Thumbprint, cert.PublicKey.Oid.FriendlyName);
            return cert;
        }

        public void Dispose()
        {
            if (_certificate.IsValueCreated)
            {
                _certificate.Value?.Dispose();
            }
        }
    }
}
