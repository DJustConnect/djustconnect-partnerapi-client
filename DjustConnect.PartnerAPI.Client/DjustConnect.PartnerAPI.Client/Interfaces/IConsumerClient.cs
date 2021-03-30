using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DjustConnect.PartnerAPI.Client.Interfaces
{
    public interface IConsumerClient
    {
        Task<PagedResult<DarStatusDTO>> GetDarStatusAsync(string farmNumberFilter);
        Task<PagedResult<DarStatusDTO>> GetDarStatusAsync(string farmNumberFilter, CancellationToken cancellationToken);
        Task<PagedResult<FarmStatusDTO>> GetFarmStatusAsync(string farmNumberFilter);
        Task<PagedResult<FarmStatusDTO>> GetFarmStatusAsync(string farmNumberFilter, CancellationToken cancellationToken);
    }
}
