using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client
{
    public class FarmMappingResult // 415
    {
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static FarmMappingResult FromJson(string data)
        {
            return JsonConvert.DeserializeObject<FarmMappingResult>(data);
        }
    }
}
