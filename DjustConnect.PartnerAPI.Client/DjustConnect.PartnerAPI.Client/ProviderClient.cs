using DjustConnect.PartnerAPI.Client.Interfaces;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjustConnect.PartnerAPI.Client
{
    public class ProviderClient : DjustConnectClient, IProviderClient
    {
        public ProviderClient(HttpClient httpClient) : base(httpClient)
        {
        }
    }
}
