using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client
{
    public class ResourceResult // Doublecheck which properties are necessary
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("providerApiName", Required = Required.Always)]
        public string ProviderApiName { get; set; }

        [JsonProperty("resourceUrl", Required = Required.AllowNull)] // AllowNull, null bij resultset?
        public string ResourceUrl { get; set; }
        //[JsonProperty("type", Required = Required.Always)]
        //public string Type { get; set; } // Enum? "type": "Pull" of "Push"

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static ResourceResult FromJson(string data)
        {
            return JsonConvert.DeserializeObject<ResourceResult>(data);
        }
    }
}
