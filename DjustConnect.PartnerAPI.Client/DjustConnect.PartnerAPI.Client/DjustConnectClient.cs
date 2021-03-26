using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client
{
    public class DjustConnectClient // base? session? http?
    {
        //TODO How to handle the ACC base url?
        public string BaseUrl { get; set; } = "https://partnerapi.djustconnect.be/";
        protected HttpClient _httpClient;
        //protected Lazy<Newtonsoft.Json.JsonSerializerSettings> _settings;
        //protected Newtonsoft.Json.JsonSerializerSettings JsonSerializerSettings { get { return _settings.Value; } }

        public DjustConnectClient(HttpClient httpClient) : base(httpClient) // no constructor with 1 args
        {
            _httpClient = httpClient;
            //_settings = new System.Lazy<Newtonsoft.Json.JsonSerializerSettings>(() =>
            //{
            //    var settings = new Newtonsoft.Json.JsonSerializerSettings();
            //    UpdateJsonSerializerSettings(settings);
            //    return settings;
            //});
        }

        public static HttpClient CreateHttpClient(string thumbprint, string subscriptionkey)
        {
            var store = new X509Store("My", StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, session.CertThumbprint, false); // No session in consumercsharp
            if (certificates.Count == 0)
            {
                throw new InvalidOperationException($"Certificate not found for CN=client.ikmnet.be in LocalMachine/My."); // Change to correct CN
            }
            var certificate = certificates[0];
            //var certificate = new X509Certificate2(session.CertificatePath, session.CertificatePassword, X509KeyStorageFlags.MachineKeySet);
            var clientHandler = new HttpClientHandler();
            clientHandler.ClientCertificates.Add(certificate);
            clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            http = new HttpClient(clientHandler); // How do I use the http var from Program?
            http.DefaultRequestHeaders.Add("DjustConnect-Subscription-Key", subscriptionkey);
            return http;
        }
    }
}
