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
    //api/Consumer/push - GET
    //api/Consumer/push/activate - POST
    //api/Consumer/push/deactivate - POST
    public class ConsumerClient : Client, IConsumerClient
    {
        #region Constructors
        public ConsumerClient(HttpClient httpClient) : base(httpClient)
        {
        }
        public ConsumerClient(string thumbprint, string subscriptionkeyDjustConnect)
        {
            _httpClient = Client.GetHttpClientWithLocalCertificate(thumbprint, subscriptionkeyDjustConnect);
        }
        public ConsumerClient(string subscriptionkeyDjustConnect, string keyvaultname, string tenantId, string certSecretname)
        {
            _httpClient = Client.GetHttpClientWithAzureKeyvaultCertificate(subscriptionkeyDjustConnect, keyvaultname, tenantId, certSecretname);
        }
        #endregion

        // api/farmer - GET
        public async Task<string[]> GetFarmsAsync(string azureADB2C_UserID)
        {
            return await GetFarmsAsync(azureADB2C_UserID, CancellationToken.None);
        }
        public async Task<string[]> GetFarmsAsync(string azureADB2C_UserID, CancellationToken cancellationToken)
        {
            var urlBuilder = GetUrlBuilder($"/api/farmer/{azureADB2C_UserID}");
            return await CallAPI(urlBuilder, GetResult<string[]>, cancellationToken);
        }

        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        public async Task<IEnumerable<FarmMappingResultDTO>> GetFarmMappingAsync(string[] requestIDs, string[] responseIDs, string farmIDType)
        {
            var urlBuilder = GetUrlBuilder("/api/farmMapping");
            JObject o = JObject.FromObject(new
            {
                farmIds = requestIDs,
                farmIdType = farmIDType, // = KBO
                resultFarmIdTypes = responseIDs //kbo, beslagnummer, keuring spuit, pe
            });
            var jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(o);
            var request = GetRequestMessage(urlBuilder);
            request.Content = new StringContent(jsonstring, Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<FarmMappingResultDTO>>(responseData).ToList();
        }

        // api/ConsumerAccess - GET
        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        public async Task<ConsumerAccessDTO> GetConsumerAccessAsync()
        {
            return await GetConsumerAccessAsync(CancellationToken.None);
        }
        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public async Task<ConsumerAccessDTO> GetConsumerAccessAsync(CancellationToken cancellationToken)
        {
            var urlBuilder = GetUrlBuilder("/api/ConsumerAccess?");
            return await CallAPI(urlBuilder, GetResult<ConsumerAccessDTO>, cancellationToken);
        }

        // api/ConsumerAccess - POST
        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        public async Task PostConsumerAccessAsync(ConsumerAccessDTO consumerAccessDTO)
        {
            await PostConsumerAccessAsync(consumerAccessDTO, CancellationToken.None);
        }
        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public async Task PostConsumerAccessAsync(ConsumerAccessDTO consumerAccessDTO, CancellationToken cancellationToken)
        {
            var urlBuilder = GetUrlBuilder("/api/ConsumerAccess?");
            await PostAPI(urlBuilder, consumerAccessDTO, cancellationToken);
        }

        // api/FarmIdType - GET
        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        public async Task<FarmIdTypeDTO[]> GetFarmIdTypesAsync() // api/FarmIdType
        {
            return await GetFarmIdTypesAsync(CancellationToken.None);
        }

        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public async Task<FarmIdTypeDTO[]> GetFarmIdTypesAsync(CancellationToken cancellationToken)
        {
            var urlBuilder = GetUrlBuilder("/api/FarmIdType");
            return await CallAPI(urlBuilder, GetResult<FarmIdTypeDTO[]>, cancellationToken);
        }

        // api/Resource - GET
        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        public async Task<ResourceDTO[]> GetResourcesAsync()
        {
            return await GetResourcesAsync(CancellationToken.None);
        }

        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public async Task<ResourceDTO[]> GetResourcesAsync(CancellationToken cancellationToken) // api/Resource
        {
            var urlBuilder = GetUrlBuilder("/api/Resource");
            return await CallAPI(urlBuilder, GetResult<ResourceDTO[]>, cancellationToken);
        }

        // api/Consumer/resource-health - GET
        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        public async Task<ResourceHealthDTO[]> GetResourceHealthAsync(Guid? resourceId)
        {
            return await GetResourceHealthAsync(resourceId, CancellationToken.None);
        }

        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public async Task<ResourceHealthDTO[]> GetResourceHealthAsync(Guid? resourceId, CancellationToken cancellationToken) // api/Consumer/resource-health - requestURL: https://partnerapi.acc.djustconnect.cegeka.com/api/Consumer/resource-health
        {
            var urlBuilder = GetUrlBuilder("/api/Consumer/resource-health?");
            urlBuilder.UrlAppendParameter("ResourceId", resourceId);
            return await CallAPI(urlBuilder, GetResult<ResourceHealthDTO[]>, cancellationToken);
        }

        // api/RarStatus - GET
        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        public async Task<PagedResult<RarStatusDTO>> GetRarStatusAsync(RarStatusFilter filter)
        {
            return await GetRarStatusAsync(filter, CancellationToken.None);
        }

        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public async Task<PagedResult<RarStatusDTO>> GetRarStatusAsync(RarStatusFilter filter, CancellationToken cancellationToken)
        {
            var urlBuilder = GetUrlBuilder("/api/RarStatus?");
            urlBuilder.UrlAppendParameter("ResourceNameFilter", filter?.ResourceName);
            urlBuilder.UrlAppendParameter("ApiNameFilter", filter?.ApiName);
            urlBuilder.UrlAppendParameter("StatusFilter", filter?.Status);
            urlBuilder.UrlAppendParameter("ProviderNameFilter", filter?.ProviderName);
            urlBuilder.UrlAppendSorting(filter?.Sort?.ToString(), filter);
            return await CallPagedAPI<RarStatusDTO>(urlBuilder, filter, cancellationToken);
        }

        // api/DarStatus - GET
        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        public async Task<PagedResult<DarStatusDTO>> GetDarStatusAsync(DarStatusFilter filter)
        {
            return await GetDarStatusAsync(filter, CancellationToken.None);
        }

        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public async Task<PagedResult<DarStatusDTO>> GetDarStatusAsync(DarStatusFilter filter, CancellationToken cancellationToken)
        {
            var urlBuilder = GetUrlBuilder("/api/DarStatus?");
            urlBuilder.UrlAppendParameter("FarmNumberFilter", filter.FarmNumber);
            urlBuilder.UrlAppendParameter("ResourceNameFilter", filter.ResourceName);
            urlBuilder.UrlAppendParameter("ResourceIdFilter", filter.ResourceId);
            urlBuilder.UrlAppendParameter("FarmStatusFilter", filter.FarmStatus);
            urlBuilder.UrlAppendParameter("ResourceStatusFilter", filter.ResourceStatus);
            urlBuilder.UrlAppendParameter("DarStatusFilter", filter.DarStatus);
            urlBuilder.UrlAppendSorting(filter?.Sort?.ToString(), filter);
            return await CallPagedAPI<DarStatusDTO>(urlBuilder, filter, cancellationToken);
        }

        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        public async Task<PagedResult<DarStatusDTO>> GetDarStatusAsync(string farmNumberFilter)
        {
            return await GetDarStatusAsync(farmNumberFilter, CancellationToken.None);
        }

        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public async Task<PagedResult<DarStatusDTO>> GetDarStatusAsync(string farmNumberFilter, CancellationToken cancellationToken)
        {
            return await GetDarStatusAsync(new DarStatusFilter { FarmNumber = farmNumberFilter, PageSize = 100 }, cancellationToken);
        }

        // api/FarmStatus - GET
        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        public async Task<PagedResult<FarmStatusDTO>> GetFarmStatusAsync(FarmStatusFilter filter)
        {
            return await GetFarmStatusAsync(filter, CancellationToken.None);
        }

        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public async Task<PagedResult<FarmStatusDTO>> GetFarmStatusAsync(FarmStatusFilter filter, CancellationToken cancellationToken)
        {
            var urlBuilder = GetUrlBuilder("/api/FarmStatus?");
            urlBuilder.UrlAppendParameter("FarmNumberFilter", filter.FarmNumber);
            urlBuilder.UrlAppendParameter("StatusFilter", filter.Status.ToString());
            urlBuilder.UrlAppendSorting(filter?.Sort?.ToString(), filter);

            return await CallPagedAPI<FarmStatusDTO>(urlBuilder, filter, cancellationToken);
        }

        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        public async Task<PagedResult<FarmStatusDTO>> GetFarmStatusAsync(string farmNumberFilter)
        {
            return await GetFarmStatusAsync(farmNumberFilter, CancellationToken.None);
        }

        /// <exception cref="DjustConnectException">A server side error occurred.</exception>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public async Task<PagedResult<FarmStatusDTO>> GetFarmStatusAsync(string farmNumberFilter, CancellationToken cancellationToken)
        {
            return await GetFarmStatusAsync(new FarmStatusFilter { FarmNumber = farmNumberFilter }, cancellationToken);
        }

        // api/Consumer/push - GET
        public Task<NotificationResultDTO> GetPushNotificationsEndpointAsync()
        {
            return GetPushNotificationsEndpointAsync(CancellationToken.None);
        }
        public async Task<NotificationResultDTO> GetPushNotificationsEndpointAsync(CancellationToken cancellationToken)
        {
            var urlBuilder = GetUrlBuilder("/api/Consumer/push");
            var response = await CallAPI(urlBuilder, GetResult<NotificationResultDTO>, cancellationToken);
            return response;
        }

        // api/Consumer/push/activate - POST
        public async Task ActivatePushNotificationsEndpointAsync(string notificationsEndpoint)
        {
            await ActivatePushNotificationsEndpointAsync(notificationsEndpoint, CancellationToken.None);
        }
        public async Task ActivatePushNotificationsEndpointAsync(string notificationsEndpoint, CancellationToken cancellationToken)
        {
            var urlBuilder = GetUrlBuilder("/api/Consumer/push/activate");
            await PostAPI(urlBuilder, new { Endpoint = notificationsEndpoint }, cancellationToken);
        }

        // api/Consumer/push/deactivate - POST
        public async Task DeactivatePushNotificationsEndpointAsync(string notificationsEndpoint)
        {
            await DeactivatePushNotificationsEndpointAsync(notificationsEndpoint, CancellationToken.None);
        }
        public async Task DeactivatePushNotificationsEndpointAsync(string notificationsEndpoint, CancellationToken cancellationToken)
        {
            var urlBuilder = GetUrlBuilder("/api/Consumer/push/deactivate");
            await PostAPI(urlBuilder, new { Endpoint = notificationsEndpoint }, cancellationToken);
        }
    }
}
