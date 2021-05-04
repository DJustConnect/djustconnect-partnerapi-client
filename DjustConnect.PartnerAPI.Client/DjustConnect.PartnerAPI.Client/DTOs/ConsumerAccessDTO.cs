using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client.DTOs
{
    public class ConsumerAccessDTO : BaseDTO //ConsumerAccess.cs
    {
        //public Guid PartnerId { get; set; }
        //public DateTime? AccessUntil { get; set; }
        //public bool AreAllFarmsSelected { get; set; }
        //public Guid? FarmIdTypeId { get; set; }
        //public string FarmsIds { get; set; }

        //public virtual DataPartner Partner { get; set; }
        //public virtual FarmIdType FarmIdType { get; set; }
        //public virtual ICollection<ResourceAccessRequest> ResourceAccessRequests { get; set; }

        public static ConsumerAccessDTO FromJson(string data)
        {
            return JsonConvert.DeserializeObject<ConsumerAccessDTO>(data);
        }
    }
}
