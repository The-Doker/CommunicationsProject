using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;

namespace CommunicationsProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SendExchangeMessageController : ControllerBase
    {
        [HttpPost]
        public string Post(string message, string exchangeName)
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
                return "Complete with message = " + message;
            }
        }
    }
}
