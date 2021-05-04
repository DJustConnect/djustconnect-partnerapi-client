using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client.Filters
{
    public class FarmStatusFilter : PagingFilter
    {
        public FarmStatusFilter(string status = null)
        {
        }
        public string FarmNumber { get; set; }
        public string Status { get; set; } // TODO enum
        public string Sort { get; set; } // hoe toe te passen?
    }
}
