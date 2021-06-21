using DjustConnect.PartnerAPI.Client.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client
{
    public class FarmMappingDTO : BaseDTO // Error 415
    {
        [Newtonsoft.Json.JsonProperty("farmIdType", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Type { get; set; }
        [Newtonsoft.Json.JsonProperty("farmId", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Value { get; set; }
        public static FarmMappingDTO FromJson(string data)
        {
            return JsonConvert.DeserializeObject<FarmMappingDTO>(data);
        }
    }
}
