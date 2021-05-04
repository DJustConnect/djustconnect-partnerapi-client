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
        public FarmStatus Status { get; set; } 
        public Sort Sort { get; set; }
    }
    public enum FarmStatus
    {
        HasUser,
        NotFound,
        HasNoUser
    }
    public enum Sort
    {
        PartnerName, 
        PartnerId,
        FarmNumber, 
        Status
    }
}
