using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client.DTOs
{
    public enum AccessRequestStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }

    public enum FarmStatus
    {
        HasUser = 0,
        NotFound = 1,
        HasNoUser = 2
    }

    public enum DarStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2,
        NotApplicable = 3,
        NoMapping = 4,
        NoData = 5
    }
}
