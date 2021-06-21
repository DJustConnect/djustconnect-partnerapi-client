using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client.DTOs
{
    class UserFarmDTO
    {
        [Newtonsoft.Json.JsonProperty("id", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Guid Id { get; set; }
        [Newtonsoft.Json.JsonProperty("mappings", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public List<FarmMappingDTO> Mappings { get; set; }
    }
}
