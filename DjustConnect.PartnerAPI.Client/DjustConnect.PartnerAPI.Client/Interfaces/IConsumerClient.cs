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
        Task<DarStatusResult[]> GetDarStatusAsync(string farmNumberFilter);
        Task<DarStatusResult[]> GetDarStatusAsync(string farmNumberFilter, CancellationToken cancellationToken);
        Task<FarmStatusResult[]> GetFarmStatusAsync(string farmNumberFilter);
        Task<FarmStatusResult[]> GetFarmStatusAsync(string farmNumberFilter, CancellationToken cancellationToken);
    }
}
