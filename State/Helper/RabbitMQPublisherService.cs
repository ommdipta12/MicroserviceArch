using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;

namespace State.Helper
{
    public interface IRabbitMQPublisherService
    {
        void Publish(string message, string routingKey, string exchange = "");
        void Dispose();
    }

    public class RabbitMQPublisherService : IRabbitMQPublisherService
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQPublisherService(IConfiguration configuration)
        {
            _configuration = configuration;
            var uri = new Uri(configuration.GetConnectionString("RabbitMq"));
            var factory = new ConnectionFactory() { HostName = uri.Host, Port = uri.Port };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Publish(string message, string routingKey, string exchange = "")
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: exchange,
                                 routingKey: routingKey,
                                 basicProperties: null,
                                 body: body);
        }

        public void Dispose()
        {
            _channel.Close();
            _connection.Close();
        }
    }
}
