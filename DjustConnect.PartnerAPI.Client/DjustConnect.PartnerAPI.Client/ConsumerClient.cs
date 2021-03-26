using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using DjustConnect.PartnerAPI.Client.Interfaces;

namespace DjustConnect.PartnerAPI.Client
{
    public class ConsumerClient : DjustConnectClient, IConsumerClient
    {
        public ConsumerClient(HttpClient httpClient) : base(httpClient)
        {
        }
        public ConsumerClient(string thumbprint, string subscriptionkey)
        {
            _httpClient = DjustConnectClient.CreateHttpClient(thumbprint, subscriptionkey);
        }

        public Task GetResourcesAsync() // api/Resource - requestURL: https://partnerapi.acc.djustconnect.cegeka.com/api/Resource
        {
            return null; // returns a list/array of all Resources
        }
        public Task GetFarmIdTypesAsync() // api/FarmIdType - requestURL: https://partnerapi.acc.djustconnect.cegeka.com/api/FarmIdType
        {
            return null; // returns a list/array of all FarmIdTypes
        }
        public Task GetFarmMappingAsync() // api/FarmMapping - requestURL: https://partnerapi.acc.djustconnect.cegeka.com/api/FarmMapping
        {
            return null; // returns farm mapping - 415 Unsupported Media Type
        }
        public Task GetResourceHealthAsync() // api/Consumer/resource-health - requestURL: https://partnerapi.acc.djustconnect.cegeka.com/api/Consumer/resource-health
        {
            return null; // returns the current health of the resource you have access to
        }


        /* TODO first next step(s):
			
			api/RarStatus
			api/ConsumerAccess - GET
			api/ConsumerAccess - POST
			
			api/Consumer/push - GET
			api/Consumer/push/activate - POST
			api/Consumer/push/deactivate - POST
		*/


        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        public Task<DarStatusDTO[]> GetDarStatusAsync(string farmNumberFilter)
        {
            return GetDarStatusAsync(farmNumberFilter, CancellationToken.None);
        }

        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public async Task<DarStatusDTO[]> GetDarStatusAsync(string farmNumberFilter, CancellationToken cancellationToken)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/DarStatus?PageSize=100&"); // paged result, paging parameters?
            urlBuilder_.Append("FarmNumberFilter=").Append(Uri.EscapeDataString(farmNumberFilter != null ? ConvertToString(farmNumberFilter, System.Globalization.CultureInfo.InvariantCulture) : "")).Append("&");
            urlBuilder_.Length--;

            var client_ = _httpClient;
            try
            {
                using (var request_ = new HttpRequestMessage())
                {
                    request_.Method = new HttpMethod("GET");
                    request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                    
                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
                    

                    var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    try
                    {
                        var headers_ = Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        

                        var status_ = ((int)response_.StatusCode).ToString();
                        if (status_ == "200")
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            try
                            {
                                var result_ = Newtonsoft.Json.JsonConvert.DeserializeObject<DarStatusDTO[]>(responseData_);
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

        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        public Task<FarmStatusDTO[]> GetFarmStatusAsync(string farmNumberFilter)
        {
            return GetFarmStatusAsync(farmNumberFilter, CancellationToken.None);
        }

        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public async Task<FarmStatusDTO[]> GetFarmStatusAsync(string farmNumberFilter, CancellationToken cancellationToken)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/FarmStatus?");
            urlBuilder_.Append("FarmNumberFilter=").Append(Uri.EscapeDataString(farmNumberFilter != null ? ConvertToString(farmNumberFilter, System.Globalization.CultureInfo.InvariantCulture) : "")).Append("&");
            urlBuilder_.Length--;

            var client_ = _httpClient;
            try
            {
                using (var request_ = new HttpRequestMessage())
                {
                    request_.Method = new HttpMethod("GET");
                    request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);
                   
                    var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    try
                    {
                        var headers_ = Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        var status_ = ((int)response_.StatusCode).ToString();
                        if (status_ == "200")
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync().ConfigureAwait(false);
                            try
                            {
                                var result_ = Newtonsoft.Json.JsonConvert.DeserializeObject<FarmStatusDTO[]>(responseData_);
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

        private string ConvertToString(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value is Enum)
            {
                string name = Enum.GetName(value.GetType(), value);
                if (name != null)
                {
                    var field = System.Reflection.IntrospectionExtensions.GetTypeInfo(value.GetType()).GetDeclaredField(name);
                    if (field != null)
                    {
                        var attribute = System.Reflection.CustomAttributeExtensions.GetCustomAttribute(field, typeof(System.Runtime.Serialization.EnumMemberAttribute))
                            as System.Runtime.Serialization.EnumMemberAttribute;
                        if (attribute != null)
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
            else if (value is byte[])
            {
                return Convert.ToBase64String((byte[])value);
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
