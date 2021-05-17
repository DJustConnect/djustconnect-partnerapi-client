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
        Task<FarmIdTypeDTO[]> GetFarmIdTypesAsync();
        Task<FarmIdTypeDTO[]> GetFarmIdTypesAsync(CancellationToken cancellationToken);
        Task<ResourceDTO[]> GetResourcesAsync();
        Task<ResourceDTO[]> GetResourcesAsync(CancellationToken cancellationToken);
        Task PostConsumerAccessAsync();
        Task PostConsumerAccessAsync(CancellationToken cancellationToken);
        Task<ResourceHealthDTO[]> GetResourceHealthAsync(Guid? resourceId);
        Task<ResourceHealthDTO[]> GetResourceHealthAsync(Guid? resourceId, CancellationToken cancellationToken);
        Task<PagedResult<RarStatusDTO>> GetRarStatusAsync(string resourceNameFilter);
        Task<PagedResult<RarStatusDTO>> GetRarStatusAsync(string resourceNameFilter, CancellationToken cancellationToken);
        Task<PagedResult<RarStatusDTO>> GetRarStatusAsyncWithFilter(RarStatusFilter filter);
        Task<PagedResult<RarStatusDTO>> GetRarStatusAsyncWithFilter(RarStatusFilter filter, CancellationToken cancellationToken);
        Task<PagedResult<DarStatusDTO>> GetDarStatusAsync(string farmNumberFilter);
        Task<PagedResult<DarStatusDTO>> GetDarStatusAsync(string farmNumberFilter, CancellationToken cancellationToken);
        Task<PagedResult<DarStatusDTO>> GetDarStatusAsyncWithFilter(DarStatusFilter filter);
        Task<PagedResult<DarStatusDTO>> GetDarStatusAsyncWithFilter(DarStatusFilter filter, CancellationToken cancellationToken);
        Task<PagedResult<FarmStatusDTO>> GetFarmStatusAsync(string farmNumberFilter);
        Task<PagedResult<FarmStatusDTO>> GetFarmStatusAsync(string farmNumberFilter, CancellationToken cancellationToken);
        Task<PagedResult<FarmStatusDTO>> GetFarmStatusAsyncWithFilter(FarmStatusFilter filter);
        Task<PagedResult<FarmStatusDTO>> GetFarmStatusAsyncWithFilter(FarmStatusFilter filter, CancellationToken cancellationToken);
        Task<ConsumerAccessDTO> GetConsumerAccessAsync();
        Task<ConsumerAccessDTO> GetConsumerAccessAsync(CancellationToken cancellationToken);


    }
}
