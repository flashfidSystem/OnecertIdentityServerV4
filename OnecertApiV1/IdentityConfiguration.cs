using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Authentication;
using System.Security.Claims;

namespace OnecertApiV1
{
    public class IdentityConfiguration
    {
        public static HttpClientHandler GetHandler()
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.SslProtocols = SslProtocols.Tls12;
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            return handler;
        }
    }
}
