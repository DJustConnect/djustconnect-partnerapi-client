using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client
{
    public class DarStatusResult
    {
        [JsonProperty("farmNumber", Required = Newtonsoft.Json.Required.Always)]
        public string FarmNumber { get; set; }
        [JsonProperty("resourceId", Required = Newtonsoft.Json.Required.Always)]
        public Guid ResourceId { get; set; }
        [JsonProperty("resourceName", Required = Newtonsoft.Json.Required.Always)]
        public string ResourceName { get; set; }
        [JsonProperty("farmStatus", Required = Newtonsoft.Json.Required.Always)]
        public string FarmStatus { get; set; }
        [JsonProperty("resourceStatus", Required = Newtonsoft.Json.Required.Always)]
        public string ResourceStatus { get; set; }
        [JsonProperty("darStatus", Required = Newtonsoft.Json.Required.Always)]
        public string DarStatus { get; set; }

        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
