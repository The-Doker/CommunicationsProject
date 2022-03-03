using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System;
using System.Text;

namespace CommunicationsProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SendMessageController : ControllerBase
    {
        [HttpPost]
        public string Post(string message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "testqueue",
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "testqueue",
                                     basicProperties: null,
                                     body: body);

                return "Complete with message = " + message;
            }
        }
    }
}
