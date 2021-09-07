using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DjustConnect.PartnerAPI.Client.DTOs; // RarStatusDto werkt niet zonder?
using DjustConnect.PartnerAPI.Client.Filters;

namespace DjustConnect.PartnerAPI.Client.Interfaces
{
    public interface IConsumerClient
    {
        Task<string[]> GetFarmsAsync(string AzureADB2C_UserID);
        Task<IEnumerable<FarmMappingResultDTO>> GetFarmMappingAsync(string[] requestIDs, string[] responseIDs, string farmIDType);
        Task<FarmIdTypeDTO[]> GetFarmIdTypesAsync();
        Task<FarmIdTypeDTO[]> GetFarmIdTypesAsync(CancellationToken cancellationToken);
        Task<ResourceDTO[]> GetResourcesAsync();
        Task<ResourceDTO[]> GetResourcesAsync(CancellationToken cancellationToken);
        Task PostConsumerAccessAsync(ConsumerAccessDTO consumerAccessDTO);
        Task PostConsumerAccessAsync(ConsumerAccessDTO consumerAccessDTO, CancellationToken cancellationToken);
        Task<ResourceHealthDTO[]> GetResourceHealthAsync(Guid? resourceId);
        Task<ResourceHealthDTO[]> GetResourceHealthAsync(Guid? resourceId, CancellationToken cancellationToken);
        Task<PagedResult<RarStatusDTO>> GetRarStatusAsync(RarStatusFilter filter);
        Task<PagedResult<RarStatusDTO>> GetRarStatusAsync(RarStatusFilter filter, CancellationToken cancellationToken);
        Task<PagedResult<DarStatusDTO>> GetDarStatusAsync(string farmNumberFilter);
        Task<PagedResult<DarStatusDTO>> GetDarStatusAsync(string farmNumberFilter, CancellationToken cancellationToken);
        Task<PagedResult<DarStatusDTO>> GetDarStatusAsync(DarStatusFilter filter);
        Task<PagedResult<DarStatusDTO>> GetDarStatusAsync(DarStatusFilter filter, CancellationToken cancellationToken);
        Task<PagedResult<FarmStatusDTO>> GetFarmStatusAsync(string farmNumberFilter);
        Task<PagedResult<FarmStatusDTO>> GetFarmStatusAsync(string farmNumberFilter, CancellationToken cancellationToken);
        Task<PagedResult<FarmStatusDTO>> GetFarmStatusAsync(FarmStatusFilter filter);
        Task<PagedResult<FarmStatusDTO>> GetFarmStatusAsync(FarmStatusFilter filter, CancellationToken cancellationToken);
        Task<ConsumerAccessDTO> GetConsumerAccessAsync();
        Task<ConsumerAccessDTO> GetConsumerAccessAsync(CancellationToken cancellationToken);


    }
}
