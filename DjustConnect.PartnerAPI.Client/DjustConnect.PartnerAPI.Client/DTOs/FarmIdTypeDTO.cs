using DjustConnect.PartnerAPI.Client.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client
{
    public class FarmIdTypeDTO : BaseDTO // Double check with Samuel
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        public static FarmIdTypeDTO FromJson(string data)
        {
            return JsonConvert.DeserializeObject<FarmIdTypeDTO>(data);
        }
    }
}
