using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using DjustConnect.PartnerAPI.Client.Interfaces;
using DjustConnect.PartnerAPI.Client.DTOs;
using DjustConnect.PartnerAPI.Client.Filters;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace DjustConnect.PartnerAPI.Client
{
    public class ConsumerClient : Client, IConsumerClient
    {
        #region Constructors
        public ConsumerClient(HttpClient httpClient) : base(httpClient)
        {
        }
        public ConsumerClient(string thumbprint, string subscriptionkey)
        {
            _httpClient = Client.GetHttpClientWithLocalCertificate(thumbprint, subscriptionkey);
        }
        public ConsumerClient(string thumbprint, string subscriptionkey, string keyvaultname, string tenantId, string certSecretname)
        {
            _httpClient = Client.GetHttpClientWithAzureKeyvaultCertificate(thumbprint, subscriptionkey, keyvaultname, tenantId, certSecretname);
        }
        #endregion

        //api/farmer
        public async Task<string[]> GetFarmsAsync(string azureADB2C_UserID)
        {
            var urlBuilder_ = new StringBuilder();
            var requestUrl = urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append($"/api/farmer/{azureADB2C_UserID}").ToString();
            return await CallAPI(urlBuilder_, GetResult<string[]>, CancellationToken.None);
        }

        //api/farmmapping
        /// <exception cref="DjustConnectException">A server side error occurred.</exception> // In afwachting
        public async Task<IEnumerable<FarmMappingResultDTO>> GetFarmMappingAsync(string[] requestIDs, string[] responseIDs, string farmIDType)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/farmMapping");
            JObject o = JObject.FromObject(new
            {
                farmIds = requestIDs,
                farmIdType = farmIDType,
                resultFarmIdTypes = responseIDs
            });
            var jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(o);
            var request = GetRequestMessage(urlBuilder_);
            request.Content = new StringContent(jsonstring, Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<FarmMappingResultDTO>>(responseData).ToList();
        }

        //api/consumeraccess  -GET
        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        public Task<ConsumerAccessDTO> GetConsumerAccessAsync()
        {
            return GetConsumerAccessAsync(CancellationToken.None);
        }
        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public async Task<ConsumerAccessDTO> GetConsumerAccessAsync(CancellationToken cancellationToken)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/ConsumerAccess?");

            return await CallAPI(urlBuilder_, GetResult<ConsumerAccessDTO>, cancellationToken);
        }
        //api/cosumeraccess  -POST
        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        public Task PostConsumerAccessAsync(ConsumerAccessDTO consumerAccessDTO)
        {
            return PostConsumerAccessAsync(consumerAccessDTO, CancellationToken.None);
        }
        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public async Task PostConsumerAccessAsync(ConsumerAccessDTO consumerAccessDTO, CancellationToken cancellationToken)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/ConsumerAccess?");
            await PostAPI(urlBuilder_, cancellationToken, consumerAccessDTO);
        }

        //api/farmIdType   -GET
        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        public Task<FarmIdTypeDTO[]> GetFarmIdTypesAsync() // api/FarmIdType
        {
            return GetFarmIdTypesAsync(CancellationToken.None);
        }

        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public async Task<FarmIdTypeDTO[]> GetFarmIdTypesAsync(CancellationToken cancellationToken)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/FarmIdType");
            return await CallAPI(urlBuilder_, GetResult<FarmIdTypeDTO[]>, cancellationToken);
        }

        //api/resource    -GET
        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        public Task<ResourceDTO[]> GetResourcesAsync() // api/Resource
        {
            return GetResourcesAsync(CancellationToken.None);
        }

        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public async Task<ResourceDTO[]> GetResourcesAsync(CancellationToken cancellationToken) // api/Resource
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/Resource");

            return await CallAPI(urlBuilder_, GetResult<ResourceDTO[]>, cancellationToken);
        }

        //api/consumer/resource-health   -GET  returns the current health of the resource you have access to
        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        public Task<ResourceHealthDTO[]> GetResourceHealthAsync(Guid? resourceId)
        {   
            return GetResourceHealthAsync(resourceId, CancellationToken.None);
        }
        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public async Task<ResourceHealthDTO[]> GetResourceHealthAsync(Guid? resourceId, CancellationToken cancellationToken) // api/Consumer/resource-health - requestURL: https://partnerapi.acc.djustconnect.cegeka.com/api/Consumer/resource-health
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/Consumer/resource-health?");
            urlBuilder_.Append("ResourceId=").Append(Uri.EscapeDataString(resourceId != null ? ConvertToString(resourceId, System.Globalization.CultureInfo.InvariantCulture) : "")).Append("&");
            urlBuilder_.Length--;

            return await CallAPI(urlBuilder_, GetResult<ResourceHealthDTO[]>, cancellationToken);
        }

        //api/rarstatus   -GET   NO FILTER
        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        public Task<PagedResult<RarStatusDTO>> GetRarStatusAsync(string resourceNameFilter)
        {
            return GetRarStatusAsync(resourceNameFilter, CancellationToken.None);
        }

        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public async Task<PagedResult<RarStatusDTO>> GetRarStatusAsync(string resourceNameFilter, CancellationToken cancellationToken)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/RarStatus?");
            urlBuilder_.Append("resourceNameFilter=").Append(Uri.EscapeDataString(resourceNameFilter != null ? ConvertToString(resourceNameFilter, System.Globalization.CultureInfo.InvariantCulture) : "")).Append("&");
            urlBuilder_.Length--;

            return await CallPagedAPI<RarStatusDTO>(urlBuilder_, cancellationToken);
        }

        //api/rarstatus   -GET   WITH FILTER
        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        public Task<PagedResult<RarStatusDTO>> GetRarStatusAsyncWithFilter(RarStatusFilter filter)
        {
            return GetRarStatusAsyncWithFilter(filter, CancellationToken.None);
        }

        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public async Task<PagedResult<RarStatusDTO>> GetRarStatusAsyncWithFilter(RarStatusFilter filter, CancellationToken cancellationToken)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/RarStatus?");
            UrlAppend(urlBuilder_, "resourceNameFilter", filter.ResourceName);
            UrlAppend(urlBuilder_, "statusFilter", filter.Status);
            UrlAppend(urlBuilder_, "apiNameFilter", filter.ApiName);
            UrlAppendPaging(urlBuilder_, filter);
            urlBuilder_.Length--;

            return await CallPagedAPI<RarStatusDTO>(urlBuilder_, cancellationToken);
        }

        //api/darstatus   -GET    NO FILTER
        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        public Task<PagedResult<DarStatusDTO>> GetDarStatusAsync(string farmNumberFilter)
        {
            return GetDarStatusAsync(farmNumberFilter, CancellationToken.None);
        }

        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public async Task<PagedResult<DarStatusDTO>> GetDarStatusAsync(string farmNumberFilter, CancellationToken cancellationToken)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/DarStatus?PageSize=100&");
            urlBuilder_.Append("FarmNumberFilter=").Append(Uri.EscapeDataString(farmNumberFilter != null ? ConvertToString(farmNumberFilter, System.Globalization.CultureInfo.InvariantCulture) : "")).Append("&");
            urlBuilder_.Length--;

            return await CallPagedAPI<DarStatusDTO>(urlBuilder_, cancellationToken);
        }

        //api/darstatus    -GET    WITH FILTER
        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        public Task<PagedResult<DarStatusDTO>> GetDarStatusAsyncWithFilter(DarStatusFilter filter)
        {
            return GetDarStatusAsyncWithFilter(filter, CancellationToken.None);
        }
        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public async Task<PagedResult<DarStatusDTO>> GetDarStatusAsyncWithFilter(DarStatusFilter filter, CancellationToken cancellationToken)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/DarStatus?");
            UrlAppend(urlBuilder_, "farmNumberFilter", filter.FarmNumber);
            UrlAppend(urlBuilder_, "resourceNameFilter", filter.ResourceName);
            UrlAppend(urlBuilder_, "resourceIdFilter", filter.ResourceId.ToString());
            UrlAppend(urlBuilder_, "farmStatusFilter", filter.FarmStatus);
            UrlAppend(urlBuilder_, "resourceStatusFilter", filter.ResourceStatus);
            UrlAppend(urlBuilder_, "darStatusFilter", filter.DarStatus);
            urlBuilder_.Length--;

            return await CallPagedAPI<DarStatusDTO>(urlBuilder_, cancellationToken);
        }

        //api/farmstatus   -GET    NO FILTER
        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        public Task<PagedResult<FarmStatusDTO>> GetFarmStatusAsync(string farmNumberFilter)
        {
            return GetFarmStatusAsync(farmNumberFilter, CancellationToken.None);
        }

        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public async Task<PagedResult<FarmStatusDTO>> GetFarmStatusAsync(string farmNumberFilter, CancellationToken cancellationToken)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/FarmStatus?");
            urlBuilder_.Append("FarmNumberFilter=").Append(Uri.EscapeDataString(farmNumberFilter != null ? ConvertToString(farmNumberFilter, System.Globalization.CultureInfo.InvariantCulture) : "")).Append("&");
            urlBuilder_.Length--;

            return await CallPagedAPI<FarmStatusDTO>(urlBuilder_, cancellationToken);
        }

        //api/farmstatus   -GET    WITH FILTER
        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        public Task<PagedResult<FarmStatusDTO>> GetFarmStatusAsyncWithFilter(FarmStatusFilter filter)
        {
            return GetFarmStatusAsyncWithFilter(filter, CancellationToken.None);
        }

        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public async Task<PagedResult<FarmStatusDTO>> GetFarmStatusAsyncWithFilter(FarmStatusFilter filter, CancellationToken cancellationToken)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/FarmStatus?");
            UrlAppend(urlBuilder_, "farmNumberFilter", filter.FarmNumber);
            UrlAppend(urlBuilder_, "statusFilter", filter.Status.ToString());
            // TODO Add Sort
            urlBuilder_.Length--;

            return await CallPagedAPI<FarmStatusDTO>(urlBuilder_, cancellationToken);
        }

        //api/Consumer/push -GET
        public Task<NotificationResultDTO> GetPushNotificationsEndpointAsync()
        {
            return GetPushNotificationsEndpointAsync(CancellationToken.None);
        }
        public async Task<NotificationResultDTO> GetPushNotificationsEndpointAsync(CancellationToken cancellationToken)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/Consumer/push");
            var response = await CallAPI(urlBuilder_, GetResult<NotificationResultDTO>, cancellationToken);
            return response;
        }
        //api/Consumer/push/activate -POST
        public async Task<string> ActivatePushNotificationsEndpointAsync(string notificationsEndpoint)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/Consumer/push/activate");
            JObject o = JObject.FromObject(new
            {
                Endpoint = notificationsEndpoint
            });
            var jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(o);
            var request = PostRequestMessage(urlBuilder_);
            request.Content = new StringContent(jsonstring, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(request.RequestUri, request.Content).ConfigureAwait(false);
            return response.StatusCode.ToString();

        }
        //api/Consumer/push/deactivate - POST
        public async Task<string> DeactivatePushNotificationsEndpointAsync(string notificationsEndpoint)
        {
            var urlBuilder_ = new StringBuilder();
            urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/Consumer/push/deactivate");
            JObject o = JObject.FromObject(new
            {
                Endpoint = notificationsEndpoint
            });
            var jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(o);
            var request = PostRequestMessage(urlBuilder_);
            request.Content = new StringContent(jsonstring, Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            return response.StatusCode.ToString();
        }

        #region Helpers
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

        public void UrlAppendPaging(StringBuilder urlBuilder, PagingFilter filter)
        {
            if (filter.PageSize != null)
                urlBuilder.Append("PageSize=").Append(Uri.EscapeDataString(ConvertToString(filter.PageSize.Value, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
                urlBuilder.Append("PageNumber=").Append(Uri.EscapeDataString(ConvertToString(filter.PageNumber.Value, System.Globalization.CultureInfo.InvariantCulture))).Append("&");
        }

        public void UrlAppend(StringBuilder urlBuilder, string parameterName, string parameter)
        {
            if (!string.IsNullOrEmpty(parameter))
                urlBuilder.Append($"{parameterName}=").Append(Uri.EscapeDataString(parameter)).Append("&");
        }
        #endregion
    }
}
