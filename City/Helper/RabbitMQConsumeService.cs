using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace City.Helper
{
    public class RabbitMQConsumeService : BackgroundService
    {
        private readonly IModel _channel;
        public RabbitMQConsumeService(IRabbitMQPublisherService rabbitMQPublisherService)
        {
            _channel = rabbitMQPublisherService.GetChannel();
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _channel.QueueDeclare(queue: "test",
                              durable: false,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
            };

            _channel.BasicConsume(queue: "test",
                                  autoAck: true,
                                  consumer: consumer);

            return Task.CompletedTask;
        }
    }
}
