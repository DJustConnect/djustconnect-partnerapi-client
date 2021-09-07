using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client
{
    class AzureSecretClientService
    {
        private SecretClient _client;

        public AzureSecretClientService(string keyvaultname, string tenantId)
        {
            DefaultAzureCredentialOptions options = new DefaultAzureCredentialOptions()
            {
                SharedTokenCacheTenantId = tenantId,
                VisualStudioTenantId = tenantId
            };
            _client = new SecretClient(new Uri($"https://{keyvaultname}.vault.azure.net/"),
                                        new DefaultAzureCredential(options));
        }

        public X509Certificate2 GetClientCertificateFromKeyVault(string secretname)
        {
            // Get full certificate + private key object from keyvault with secretclient
            // see --> https://github.com/Azure/azure-sdk-for-js/issues/7647
            var PKCS12 = _client.GetSecret(secretname).Value;
            var PKCS12bytes = Convert.FromBase64String(PKCS12.Value);
            //specify StorageFlags, otherwise WindowsCryptographicException when deploying to Azure
            return new X509Certificate2(
                PKCS12bytes,
                String.Empty, // omit pw
                X509KeyStorageFlags.MachineKeySet |
                X509KeyStorageFlags.PersistKeySet |
                X509KeyStorageFlags.Exportable);
        }

        public string GetSecretFromKeyVault(string secretname)
        {
            var secret = _client.GetSecret(secretname);
            return secret.Value.Value;
        }
    }
}
