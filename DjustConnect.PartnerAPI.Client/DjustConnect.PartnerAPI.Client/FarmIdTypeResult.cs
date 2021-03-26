using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client
{
    public class FarmIdTypeResult // Doublecheck which properties are necessary
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("isDefault", Required = Required.Always)]
        public bool IsDefault { get; set; }

        [JsonProperty("resourceApis", Required = Required.AllowNull)] // null in results
        public string ResourceApis { get; set; }
        [JsonProperty("farmIdMappings", Required = Required.AllowNull)] // null in results
        public string FarmIdMappings { get; set; }


        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static FarmIdTypeResult FromJson(string data)
        {
            return JsonConvert.DeserializeObject<FarmIdTypeResult>(data);
        }
    }
}
