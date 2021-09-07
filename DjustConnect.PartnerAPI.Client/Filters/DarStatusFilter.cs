using DjustConnect.PartnerAPI.Client.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client.Filters
{
    public class DarStatusFilter : PagingFilter
    {
        public DarStatusFilter(string farmNumber=null)
        {
            FarmNumber = farmNumber;
        }
        public string FarmNumber { get; set; }
        public Guid? ResourceId { get; set; }
        public string ResourceName { get; set; }
        public FarmStatus? FarmStatus { get; set; }
        public AccessRequestStatus? ResourceStatus { get; set; }
        public DarStatus? DarStatus { get; set; }
        public DarStatusSort? Sort { get; set; }
    }

    public enum DarStatusSort
    {
        FarmNumber,
        ResourceName,
        FarmStatus,
        ResourceStatus,
        DarStatus
    }
}
