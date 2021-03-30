﻿using DjustConnect.PartnerAPI.Client.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client
{
    public class ResourceDTO : BaseDTO // Double check AllowNull
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("providerApiName", Required = Required.Always)]
        public string ProviderApiName { get; set; }

        [JsonProperty("resourceUrl", Required = Required.AllowNull)] // AllowNull, null bij resultset?
        public string ResourceUrl { get; set; }

        public static ResourceDTO FromJson(string data)
        {
            return JsonConvert.DeserializeObject<ResourceDTO>(data);
        }
    }
}