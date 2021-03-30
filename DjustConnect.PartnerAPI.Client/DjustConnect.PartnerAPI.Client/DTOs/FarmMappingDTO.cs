using DjustConnect.PartnerAPI.Client.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client
{
    public class FarmMappingDTO : BaseDTO // 415
    {
        public static FarmMappingDTO FromJson(string data)
        {
            return JsonConvert.DeserializeObject<FarmMappingDTO>(data);
        }
    }
}
