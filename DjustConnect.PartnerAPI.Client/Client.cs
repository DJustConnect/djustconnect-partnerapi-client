using DjustConnect.PartnerAPI.Client.DTOs;
using DjustConnect.PartnerAPI.Client.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client
{
    public class Client
    {
        public string BaseUrl { get; set; } = "https://partnerapi.djustconnect.be/";
        protected HttpClient _httpClient;

        protected Client()
        {
        }
        public Client(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public static HttpClient GetHttpClientWithLocalCertificate(string thumbprint, string subscriptionkeyDjustConnect)
        {
            var certificate = GetCertificateFromLocalCertStore(thumbprint);
            return GetHttpClient(certificate, subscriptionkeyDjustConnect);
        }
        public static HttpClient GetHttpClientWithAzureKeyvaultCertificate(string subscriptionkeyDjustConnect, string keyvaultname, string tenantId, string certSecretname)
        {
            var certificate = GetCertificateFromKeyVault(keyvaultname, tenantId, certSecretname);
            return GetHttpClient(certificate, subscriptionkeyDjustConnect);
        }

        private static HttpClient GetHttpClient(X509Certificate2 certificate, string subscriptionkeyDjustConnect)
        {
            var clientHandler = new HttpClientHandler();
            clientHandler.ClientCertificates.Add(certificate);
            clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            var http = new HttpClient(clientHandler);
            http.DefaultRequestHeaders.Add("DjustConnect-Subscription-Key", subscriptionkeyDjustConnect);
            return http;
        }

        private static X509Certificate2 GetCertificateFromKeyVault(string keyvaultname, string tenantId, string certSecretname)
        {
            var ascs = new AzureSecretClientService(keyvaultname, tenantId);
            return ascs.GetClientCertificateFromKeyVault(certSecretname);
        }

        private static X509Certificate2 GetCertificateFromLocalCertStore(string thumbprint)
        {
            var store = new X509Store("My", StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
            if (certificates.Count == 0)
            {
                throw new InvalidOperationException($"Certificate not found in LocalMachine/My.");
            }
            return certificates[0];
        }
        

        protected StringBuilder GetUrlBuilder(string endpointPath)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append(endpointPath);
            return urlBuilder;
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
            return JsonConvert.DeserializeObject<T>(json);
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
            var url = urlbuilder.ToString();
            if (url.EndsWith("&") || url.EndsWith("?"))
            {
                urlbuilder.Length--;
                url = urlbuilder.ToString();
            }
            request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);
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

        protected async Task<System.Net.HttpStatusCode> PostAPI<T>(StringBuilder urlBuilder, T dto, CancellationToken cancellationToken)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            using (var request_ = PostRequestMessage(urlBuilder))
            {
                var response_ = await _httpClient.PostAsync(request_.RequestUri, stringContent, cancellationToken).ConfigureAwait(false); // bij debug skipt de code, hoe spring ik hier in?
                try
                {
                    var headers_ = response_.GetResponseHeaders();
                    var status_ = ((int)response_.StatusCode).ToString();
                    if (status_ != "200" && status_ != "204")
                    {
                        throw new DjustConnectException("The HTTP status code of the response was not expected (" + (int)response_.StatusCode + ").", (int)response_.StatusCode, null, headers_, null);
                    }
                    return response_.StatusCode;
                }
                finally
                {
                    if (response_ != null)
                        response_.Dispose();
                }
            }
        }

        protected async Task<PagedResult<T>> CallPagedAPI<T>(StringBuilder urlBuilder, PagingFilter filter, CancellationToken cancellationToken)
        {
            urlBuilder.UrlAppendPaging(filter);
            return await CallAPI(urlBuilder, GetPagedResult<T>, cancellationToken);
        }

        protected async Task<T> CallAPI<T>(StringBuilder urlBuilder, CancellationToken cancellationToken) where T : class
        {
            return await CallAPI(urlBuilder, GetResult<T>, cancellationToken);
        }

        protected async Task<T> CallAPI<T>(StringBuilder urlBuilder, Func<Dictionary<string, IEnumerable<string>>, string, T> getResult, CancellationToken cancellationToken) where T : class
        {
            try
            {
                using (var request_ = GetRequestMessage(urlBuilder))
                {
                    var response_ = await _httpClient.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    try
                    {
                        var headers_ = response_.GetResponseHeaders();
                        var status_ = ((int)response_.StatusCode).ToString();
                        if (status_ == "200")
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
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
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
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
        internal static Dictionary<string, IEnumerable<string>> GetResponseHeaders(this HttpResponseMessage response_)
        {
            var headers_ = Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
            if (response_.Content != null && response_.Content.Headers != null)
            {
                foreach (var item_ in response_.Content.Headers)
                    headers_[item_.Key] = item_.Value;
            }
            return headers_;
        }

        internal static void UrlAppendPaging(this StringBuilder urlBuilder, PagingFilter filter)
        {
            if (filter != null)
            {
                urlBuilder.UrlAppendParameter("PageSize", filter.PageSize);
                urlBuilder.UrlAppendParameter("PageNumber", filter.PageNumber);
            }
        }

        internal static void UrlAppendSorting(this StringBuilder urlBuilder, string sortField, PagingFilter filter)
        {
            if (!string.IsNullOrWhiteSpace(sortField))
            {
                if (filter.SortDirection == SortDirection.Descending)
                    sortField += "-desc";

                urlBuilder.UrlAppendParameter("sort", sortField);
            }
        }

        internal static void UrlAppendParameter(this StringBuilder urlBuilder, string parameterName, object parameter)
        {
            if (parameter != null)
            {
                if (parameter is not string strparam)
                {
                    strparam = ConvertToString(parameter, System.Globalization.CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(strparam))
                    urlBuilder.Append($"{parameterName}=").Append(Uri.EscapeDataString(strparam)).Append('&');
            }
        }

        private static string ConvertToString(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value is Enum)
            {
                string name = Enum.GetName(value.GetType(), value);
                if (name != null)
                {
                    var field = System.Reflection.IntrospectionExtensions.GetTypeInfo(value.GetType()).GetDeclaredField(name);
                    if (field != null)
                    {
                        if (System.Reflection.CustomAttributeExtensions.GetCustomAttribute(field, typeof(System.Runtime.Serialization.EnumMemberAttribute)) is System.Runtime.Serialization.EnumMemberAttribute attribute)
                        {
                            return attribute.Value;
                        }
                    }
                }
            }
            else if (value is bool)
            {
                return Convert.ToString(value, cultureInfo).ToLowerInvariant();
            }
            else if (value is byte[] v)
            {
                return Convert.ToBase64String(v);
            }
            else if (value != null && value.GetType().IsArray)
            {
                var array = Enumerable.OfType<object>((Array)value);
                return string.Join(",", Enumerable.Select(array, o => ConvertToString(o, cultureInfo)));
            }

            return Convert.ToString(value, cultureInfo);
        }
    }
}
