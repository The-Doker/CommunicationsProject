using RabbitMQ.Client;
using System.Text;

namespace CommunicationsProject.Producer
{
    public class RabbitMessageProducer : IMessageProducer
    {
        public void SendToBroker(string message, string exchangeName)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);

                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: exchangeName,
                                     routingKey: "",
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
