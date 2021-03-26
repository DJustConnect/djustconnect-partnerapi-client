using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Text.Json;
using WebAPIClient;

namespace PartnerAPIClient
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            await GetResourceHealth();

            //await ProcessStringResponse();

            //var repositories = await ProcessRepositories();

            //foreach (var repo in repositories)
            //{
            //    Console.WriteLine(repo.Name);
            //    Console.WriteLine(repo.Description);
            //    Console.WriteLine(repo.GitHubHomeUrl);
            //    Console.WriteLine(repo.Homepage);
            //    Console.WriteLine(repo.Watchers);
            //    Console.WriteLine(repo.LastPush);
            //    Console.WriteLine();
            //}
        }

        private static async Task<List<Repository>> ProcessRepositories()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var streamTask = client.GetStreamAsync("https://api.github.com/orgs/dotnet/repos");
            var repositories = await JsonSerializer.DeserializeAsync<List<Repository>>(await streamTask);
            return repositories;
        }

        private static async Task ProcessStringResponse()
        {
            var stringResponse = client.GetStringAsync("https://partnerapi.acc.djustconnect.cegeka.com/v1/swagger.json");
            var message = await stringResponse;
            Console.Write(message);
        }

        private static async Task GetResourceHealth()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://partnerapi.acc.djustconnect.cegeka.com/v1/");

                var stringTask = client.GetAsync("/api/Consumer/resource-health");
                var message = stringTask.Result;
                Console.Write(message);
            }
        }
    }
}
