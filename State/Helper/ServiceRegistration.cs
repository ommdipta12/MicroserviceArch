using Consul;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;
using State.Models;


namespace State.Helper
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddConsulConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConsulClient, ConsulClient>(p => 
                new ConsulClient(consulConfig =>
                    {
                        var address = configuration.GetValue<string>("Consul:Host");
                        consulConfig.Address = new Uri(address);
                })
            );
            services.Configure<ConsulConfig>(configuration.GetSection("Consul:Register"));
            //services.AddHostedService<ConsulHostedService>();
            return services;
        }

        public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
        {
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var lifetime = app.ApplicationServices.GetRequiredService<Microsoft.AspNetCore.Hosting.IApplicationLifetime>();
            
            var address = $"https://localhost:44377";

            var uri = new Uri(address);
            if (uri != null)
            {
                var registration = new AgentServiceRegistration()
                {
                    ID = $"StateService-{uri.Port}", //$"MyService-57084",
                    // servie name  
                    Name = "StateService",
                    Address = $"{uri.Host}", //"localhost",
                    Port = uri.Port  // uri.Port
                };

                consulClient.Agent.ServiceDeregister(registration.ID).GetAwaiter().GetResult();
                consulClient.Agent.ServiceRegister(registration).GetAwaiter().GetResult();

                lifetime.ApplicationStopping.Register(async () =>
                {
                    await consulClient.Agent.ServiceDeregister(registration.ID);

                    System.Threading.Thread.Sleep(1000);
                });
            }

            return app;

        }
    }
}
