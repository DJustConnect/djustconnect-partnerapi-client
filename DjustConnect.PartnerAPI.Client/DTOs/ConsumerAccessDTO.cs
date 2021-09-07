using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client.DTOs
{
    public class ConsumerAccessDTO : BaseDTO 
    {
        [JsonProperty("accessUntil", Required = Required.AllowNull)]
        public DateTime? AccessUntil { get; set; }
        [JsonProperty("farmsIds", Required = Required.Always)]
        public ICollection<string> FarmsIds { get; set; }
        [JsonProperty("resources", Required = Required.Always)]
        public ICollection<ResourceDTO> Resources { get; set; }
        [JsonProperty("farmIdType", Required = Required.Always)]
        public FarmIdTypeDTO FarmIdType { get; set; }

        public static ConsumerAccessDTO FromJson(string data)
        {
            return JsonConvert.DeserializeObject<ConsumerAccessDTO>(data);
        }
        public static string ToJson(ConsumerAccessDTO consumerAccessDTO)
        {
            return JsonConvert.SerializeObject(consumerAccessDTO);
        }
    }
}
