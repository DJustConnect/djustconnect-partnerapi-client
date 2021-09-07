using DjustConnect.PartnerAPI.Client.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client.Filters
{
    public class FarmStatusFilter : PagingFilter
    {
        public FarmStatusFilter(FarmStatus? status=null)
        {
            Status = status;
        }
        public string FarmNumber { get; set; }
        public FarmStatus? Status { get; set; } 
        public FarmStatusSort? Sort { get; set; }
    }
    
    public enum FarmStatusSort
    {
        FarmNumber, 
        Status
    }
}
