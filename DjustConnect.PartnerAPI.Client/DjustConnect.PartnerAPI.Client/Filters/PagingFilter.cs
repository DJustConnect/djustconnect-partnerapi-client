using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client.Filters
{
    public abstract class PagingFilter
    {
        public int? PageSize { get; set; } // why is this nullable? (original)
        public int? PageNumber { get; set; }
        public int? Pages { get; set; } // useful?
        public int? TotalCount { get; set; }
    }
}
