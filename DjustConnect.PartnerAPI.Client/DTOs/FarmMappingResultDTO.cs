using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client.DTOs
{
    public class FarmMappingResultDTO
    {
        [Newtonsoft.Json.JsonProperty("farmId", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string FarmId { get; set; }
        [Newtonsoft.Json.JsonProperty("farmIdType", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string FarmIdType { get; set;}
        [Newtonsoft.Json.JsonProperty("mappings", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public FarmMappingDTO[] FarmMappings { get; set; }
    }
}
