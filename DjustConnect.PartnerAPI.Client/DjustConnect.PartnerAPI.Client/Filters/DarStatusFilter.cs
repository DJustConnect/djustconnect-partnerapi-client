using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client.Filters
{
    public class DarStatusFilter : PagingFilter
    {
        public DarStatusFilter(string resourceName = null, string darStatus = null)
        {

        }
        public string FarmNumber { get; set; }
        public Guid? ResourceId { get; set; }
        public string ResourceName { get; set; }
        public string FarmStatus { get; set; }
        public string ResourceStatus { get; set; }
        public string DarStatus { get; set; }

    }
}
