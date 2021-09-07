using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client.Filters
{
    public class RarStatusFilter : PagingFilter
    {
        public string ResourceName { get; set; }
        public string Status { get; set; }
        public string ApiName { get; set; }
        public string PartnerName { get; set; }
        public string ProviderName { get; set; }
    }
}
