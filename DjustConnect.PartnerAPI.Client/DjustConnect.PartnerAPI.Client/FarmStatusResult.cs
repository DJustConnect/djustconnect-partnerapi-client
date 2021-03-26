using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client
{
    public class FarmStatusResult
    {
        [JsonProperty("farmNumber", Required = Newtonsoft.Json.Required.Always)]
        public string FarmNumber { get; set; }
        [JsonProperty("status", Required = Newtonsoft.Json.Required.Always)]
        public string Status { get; set; }

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
