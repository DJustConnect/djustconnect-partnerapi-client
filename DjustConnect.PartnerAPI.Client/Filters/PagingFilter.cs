using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client.Filters
{
    public abstract class PagingFilter
    {
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }

    }
}
