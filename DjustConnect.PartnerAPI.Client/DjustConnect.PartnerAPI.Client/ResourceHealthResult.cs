using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client
{
    public class ResourceHealthResult
    {
        [JsonProperty("resourceName", Required = Required.Always)]
        public string ResourceName { get; set; }

        [JsonProperty("lastResponse", Required = Required.Always)]
        public string LastResponse { get; set; }

        [JsonProperty("lastOkResponse", Required = Required.Always)]
        public string LastOkResponse { get; set; }

        [JsonProperty("resourceHealth", Required = Required.Always)]
        public string ResourceHealth { get; set; } // string? result = "Unknown"

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static ResourceHealthResult FromJson(string data)
        {
            return JsonConvert.DeserializeObject<ResourceHealthResult>(data);
        }
    }
}
