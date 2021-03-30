using DjustConnect.PartnerAPI.Client.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client
{
    public class DarStatusDTO : BaseDTO
    {
        [JsonProperty("farmNumber", Required = Required.Always)]
        public string FarmNumber { get; set; }
        [JsonProperty("resourceId", Required = Required.Always)]
        public Guid ResourceId { get; set; } //Guid ipv string op swagger
        [JsonProperty("resourceName", Required = Required.Always)]
        public string ResourceName { get; set; }
        [JsonProperty("farmStatus", Required = Required.Always)]
        public string FarmStatus { get; set; }
        [JsonProperty("resourceStatus", Required = Required.Always)]
        public string ResourceStatus { get; set; }
        [JsonProperty("darStatus", Required = Required.Always)]
        public string DarStatus { get; set; }

        public static DarStatusDTO FromJson(string data)
        {
            return JsonConvert.DeserializeObject<DarStatusDTO>(data);
        }
    }
}
