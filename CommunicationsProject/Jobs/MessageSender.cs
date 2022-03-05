using CommunicationsProject.Producer;
using Quartz;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationsProject.Jobs
{
    public class MessageSender : IJob
    {
        private readonly IMessageProducer _producer;
        public MessageSender(IMessageProducer producer)
        {
            _producer = producer;
        }
        public Task Execute(IJobExecutionContext context)
        {
            var exchangeName = "group-one";
            var message = "This is message from Quartz.NET";
            _producer.SendToBroker(message, exchangeName);
            return Task.CompletedTask;
        }
    }
}
