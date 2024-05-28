using Consul;
using Newtonsoft.Json;
using System;

namespace City.Services
{
    public class ConsulHttpClient : IConsulHttpClient
    {
        private readonly IConsulClient _consulclient;

        public ConsulHttpClient(IConsulClient consulclient)
        {
            _consulclient = consulclient;
        }

        public async Task<T> GetAsync<T>(string serviceName, string requestPath)
        {
            var uri = await GetRequestUriAsync(serviceName, requestPath);

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                if (!response.IsSuccessStatusCode)
                {
                    return default(T);
                }

                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
        }

        private async Task<Uri> GetRequestUriAsync(string serviceName, string requestPath)
        {
            //Get all services registered on Consul
            var allRegisteredServices = await _consulclient.Agent.Services();

            //Get all instance of the service went to send a request to
            var registeredServices = allRegisteredServices.Response?.Where(s => s.Value.Service.Equals(serviceName, StringComparison.OrdinalIgnoreCase)).Select(x => x.Value).ToList();

            //Get a random instance of the service
            var service = GetRandomInstance(registeredServices, serviceName);

            if (service == null)
            {
                throw new Exception($"Consul service: '{serviceName}' was not found.");
            }

            var uriBuilder = new Uri(new Uri($"https://{service.Address}:{service.Port}"), requestPath);

            return uriBuilder;
        }

        private AgentService GetRandomInstance(IList<AgentService> services, string serviceName)
        {
            Random _random = new Random();

            AgentService servToUse = null;

            servToUse = services[_random.Next(0, services.Count)];

            return servToUse;
        }
    }
}
