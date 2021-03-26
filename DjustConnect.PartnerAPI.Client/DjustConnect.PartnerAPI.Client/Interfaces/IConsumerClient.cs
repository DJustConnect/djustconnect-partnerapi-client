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
        Task<DarStatusDTO[]> GetDarStatusAsync(string farmNumberFilter);
        Task<DarStatusDTO[]> GetDarStatusAsync(string farmNumberFilter, CancellationToken cancellationToken);
        Task<FarmStatusDTO[]> GetFarmStatusAsync(string farmNumberFilter);
        Task<FarmStatusDTO[]> GetFarmStatusAsync(string farmNumberFilter, CancellationToken cancellationToken);
    }
}
