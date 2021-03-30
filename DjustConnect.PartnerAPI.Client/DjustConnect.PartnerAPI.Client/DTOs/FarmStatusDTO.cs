using DjustConnect.PartnerAPI.Client.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client
{
    public class FarmStatusDTO : BaseDTO
    {
        [JsonProperty("farmNumber", Required = Required.Always)]
        public string FarmNumber { get; set; }
        [JsonProperty("status", Required = Required.Always)]
        public string Status { get; set; }

        public static FarmStatusDTO FromJson(string data)
        {
            return JsonConvert.DeserializeObject<FarmStatusDTO>(data);
        }
    }
}
