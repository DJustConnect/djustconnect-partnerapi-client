using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client.DTOs
{
    public class NotificationResultDTO : BaseDTO
    {
        [JsonProperty("receiveNotifications", Required = Required.AllowNull)]
        public bool ReceiveNotifications { get; set; }
        [JsonProperty("endpoint", Required = Required.AllowNull)]
        public string Endpoint { get; set; }
        [JsonProperty("subscriptionStatus", Required = Required.AllowNull)]
        public string SubscriptionStatus { get; set; }
        public static NotificationResultDTO FromJson(string data)
        {
            return JsonConvert.DeserializeObject<NotificationResultDTO>(data);
        }
    }
}
