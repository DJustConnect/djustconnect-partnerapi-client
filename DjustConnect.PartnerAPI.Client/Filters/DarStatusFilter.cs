using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client.Filters
{
    public class DarStatusFilter : PagingFilter
    {
        public string FarmNumber { get; set; }
        public Guid? ResourceId { get; set; }
        public string ResourceName { get; set; }
        public string FarmStatus { get; set; } // TODO enum
        public string ResourceStatus { get; set; } // TODO enum
        public string DarStatus { get; set; } // TODO enum

    }
}
