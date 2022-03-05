using Quartz;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationsProject.Jobs
{
    public class MessageSender : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            var exchangeName = "group-one";
            var message = "This is message from Quartz.NET";
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
            return Task.CompletedTask;
        }
    }
}
