using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client
{
    public class ResourceResult
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }
        [JsonProperty("providerApiName", Required = Required.Always)]
        public string ProviderApiName { get; set; }
        [JsonProperty("resourceUrl", Required = Required.Always)]
        public string ResourceUrl { get; set; }
        //[JsonProperty("type", Required = Required.Always)] // Comes from FarmIdTypes, do this first?
        //public string Type { get; set; } // Enum?

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static FarmStatusResult FromJson(string data)
        {
            return JsonConvert.DeserializeObject<FarmStatusResult>(data);
        }
    }
}
