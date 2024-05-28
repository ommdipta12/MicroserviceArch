using Consul;
using City.Models;

namespace City.Helper
{
    public class ConsulHostedService : IHostedService
    {
        private readonly IConsulClient _consulClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ConsulHostedService> _logger;

        public ConsulHostedService(IConsulClient consulClient, IConfiguration configuration, ILogger<ConsulHostedService> logger)
        {
            _consulClient = consulClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var serviceConfig = _configuration.GetSection("consul").Get<ConsulConfig>();
            var registration = new AgentServiceRegistration
            {
                ID = serviceConfig.ServiceId,
                Name = serviceConfig.ServiceName,
                Address = serviceConfig.ServiceHost,
                Port = serviceConfig.ServicePort
            };

            var check = new AgentServiceCheck
            {
                HTTP = serviceConfig.HealthCheckUrl,
                Interval = TimeSpan.FromSeconds(serviceConfig.HealthCheckIntervalSeconds),
                Timeout = TimeSpan.FromSeconds(serviceConfig.HealthCheckTimeoutSeconds)
            };

            registration.Checks = new[] { check };

            _logger.LogInformation($"Registering service with Consul: {registration.Name}");

            await _consulClient.Agent.ServiceDeregister(registration.ID, cancellationToken);
            await _consulClient.Agent.ServiceRegister(registration, cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            var serviceConfig = _configuration.GetSection("consul").Get<ConsulConfig>();
            var registration = new AgentServiceRegistration { ID = serviceConfig.ServiceId };

            _logger.LogInformation($"Deregistering service from Consul: {registration.ID}");

            await _consulClient.Agent.ServiceDeregister(registration.ID, cancellationToken);
        }
    }
}
