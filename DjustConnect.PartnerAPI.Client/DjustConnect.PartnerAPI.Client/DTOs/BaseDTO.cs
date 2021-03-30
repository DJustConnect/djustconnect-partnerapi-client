using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client.DTOs
{
    public class BaseDTO
    {
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
