using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace DjustConnect.PartnerAPI.Client
{
    public class Program
    {
        public void Main() // How to use this http var?
		{
            var http = DjustConnectClient.CreateHttpClient("thumbprint here", "subscription key here");
            var consumerclient = new ConsumerClient(http);
            var darstatusresults = consumerclient.GetDarStatusAsync("farm number here").Result;
            // TODO do something with dar status result
        }
	}
}
