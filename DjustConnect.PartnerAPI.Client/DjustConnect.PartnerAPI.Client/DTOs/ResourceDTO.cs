using DjustConnect.PartnerAPI.Client.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client
{
    public class ResourceDTO : BaseDTO
    {
        [JsonProperty("id", Required = Required.Always)]
        public Guid Id { get; set; }
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("providerApiName", Required = Required.Always)]
        public string ProviderApiName { get; set; }

        [JsonProperty("resourceUrl", Required = Required.AllowNull)]
        public string ResourceUrl { get; set; }
        [JsonProperty("type", Required = Required.AllowNull)]
        public string Type { get; set; }

        public static ResourceDTO FromJson(string data)
        {
            return JsonConvert.DeserializeObject<ResourceDTO>(data);
        }
    }
}
