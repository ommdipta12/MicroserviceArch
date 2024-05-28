using Consul;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;


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

                consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
                consulClient.Agent.ServiceRegister(registration).ConfigureAwait(true);

                lifetime.ApplicationStopping.Register(() =>
                {
                    consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
                });
            }

            return app;

        }
    }
}
