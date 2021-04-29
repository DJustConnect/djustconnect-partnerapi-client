using DjustConnect.PartnerAPI.Client.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client
{
    public class DjustConnectClient //TODO use PagedResult as helper method here
    {
        public string BaseUrl { get; set; } = "https://partnerapi.djustconnect.be/";
        protected HttpClient _httpClient;

        protected DjustConnectClient()
        {
        }
        public DjustConnectClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public static HttpClient CreateHttpClient(string thumbprint, string subscriptionkey)
        {
            var store = new X509Store("My", StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false); 
            if (certificates.Count == 0)
            {
                throw new InvalidOperationException($"Certificate not found for CN=consumercsharp in LocalMachine/My.");
            }
            var certificate = certificates[0];
            //var certificate = new X509Certificate2(session.CertificatePath, session.CertificatePassword, X509KeyStorageFlags.MachineKeySet);
            var clientHandler = new HttpClientHandler();
            clientHandler.ClientCertificates.Add(certificate);
            clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            var http = new HttpClient(clientHandler);
            http.DefaultRequestHeaders.Add("DjustConnect-Subscription-Key", subscriptionkey);
            return http;
        }
        public static void AddPaging()
        {
            // Add paging to a DTO
        }

        public static PagedResult<T> GetPagedResult<T>(Dictionary<string, IEnumerable<string>> headers, T[] result)
        {
            return new PagedResult<T>
            {
                PageNumber = headers["X-PageNumber"].Select(x => Convert.ToInt32(x)).Single(),
                Pages = headers["X-Pages"].Select(x => Convert.ToInt32(x)).Single(),
                PageSize = headers["X-PageSize"].Select(x => Convert.ToInt32(x)).Single(),
                TotalCount = headers["X-TotalCount"].Select(x => Convert.ToInt32(x)).Single(),
                Result = result
            };
        }

        protected static Dictionary<string, IEnumerable<string>> GetResponseHeaders(HttpResponseMessage response_)
        {
            var headers_ = Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null)
            {
                foreach (var item_ in response_.Content.Headers)
                    headers_[item_.Key] = item_.Value;
            }
            return headers_;
        }

    }
}
