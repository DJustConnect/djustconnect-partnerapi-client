using DjustConnect.PartnerAPI.Client.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client
{
    public class DjustConnectClient 
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
        }

        public static PagedResult<T> GetPagedResult<T>(Dictionary<string, IEnumerable<string>> headers, string json)
        {
            return new PagedResult<T>
            {
                PageNumber = headers["X-PageNumber"].Select(x => Convert.ToInt32(x)).Single(),
                Pages = headers["X-Pages"].Select(x => Convert.ToInt32(x)).Single(),
                PageSize = headers["X-PageSize"].Select(x => Convert.ToInt32(x)).Single(),
                TotalCount = headers["X-TotalCount"].Select(x => Convert.ToInt32(x)).Single(),
                Result = Newtonsoft.Json.JsonConvert.DeserializeObject<T[]>(json)
            };
        }
        public static T GetResult<T>(Dictionary<string, IEnumerable<string>> headers, string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
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

        protected static HttpRequestMessage GetRequestMessage(StringBuilder urlbuilder)
        {
            var request = new HttpRequestMessage();

            request.Method = new HttpMethod("GET");
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

            var url_ = urlbuilder.ToString();
            request.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
            return request;
        }
        protected static HttpRequestMessage PostRequestMessage(StringBuilder urlbuilder)
        {
            var request = new HttpRequestMessage();

            request.Method = new HttpMethod("POST");
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

            var url_ = urlbuilder.ToString();
            request.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
            return request;
        }

        protected async Task<PagedResult<T>> CallPagedAPI<T>(StringBuilder urlBuilder, CancellationToken cancellationToken)
        {
            return await CallAPI(urlBuilder, GetPagedResult<T>, cancellationToken);
        }

        protected async Task<T> CallAPI<T>(StringBuilder urlBuilder, Func<Dictionary<string, IEnumerable<string>>, string, T> getResult, CancellationToken cancellationToken) where T : class
        {
            var client_ = _httpClient;
            try
            {
                using (var request_ = GetRequestMessage(urlBuilder))
                {
                    var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    try
                    {
                        var headers_ = response_.GetResponseHeaders();
                        // TODO var status = response_.GetStatusCode();
                        //var headers_ = GetResponseHeaders(response_);

                        var status_ = ((int)response_.StatusCode).ToString();
                        if (status_ == "200")
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            try
                            {
                                var result_ = getResult(headers_, responseData_);
                                return result_;
                            }
                            catch (Exception exception_)
                            {
                                throw new DjustConnectException("Could not deserialize the response body.", (int)response_.StatusCode, responseData_, headers_, exception_);
                            }
                        }
                        else if (status_ != "200" && status_ != "204")
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new DjustConnectException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, responseData_, headers_, null);
                        }
                        return null;
                    }
                    finally
                    {
                        if (response_ != null)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
            }
        }
    }

    internal static class Extensions
    {
        public static Dictionary<string, IEnumerable<string>> GetResponseHeaders(this HttpResponseMessage response_)
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
