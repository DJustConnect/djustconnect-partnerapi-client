using DjustConnect.PartnerAPI.Client.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client
{
    public class ResourceHealthDTO : BaseDTO
    {
        [JsonProperty("resourceName", Required = Required.Always)]
        public string ResourceName { get; set; }

        [JsonProperty("resourceHealth", Required = Required.Always)]
        public string ResourceHealth { get; set; } // string? result = "Unknown"

        public static ResourceHealthDTO FromJson(string data)
        {
            return JsonConvert.DeserializeObject<ResourceHealthDTO>(data);
        }
    }
}
