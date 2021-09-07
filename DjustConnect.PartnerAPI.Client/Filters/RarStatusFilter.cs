using DjustConnect.PartnerAPI.Client.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client.Filters
{
    public class RarStatusFilter : PagingFilter
    {
        public RarStatusFilter()
        {
        }

        public string ResourceName { get; set; }
        public AccessRequestStatus? Status { get; set; }
        public string ApiName { get; set; }
        public string PartnerName { get; set; }
        public string ProviderName { get; set; }
        public RarStatusSort? Sort { get; set; }
    }

    public enum RarStatusSort
    {
        ResourceName,
        ApiName,
        ProviderName,
        Status
    }
}
