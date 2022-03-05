using CommunicationsProject.Producer;
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
        private readonly IMessageProducer _producer;
        public SendExchangeMessageController(IMessageProducer producer)
        {
            _producer = producer;
        }

        [HttpPost]
        public string Post(string message, string exchangeName)
        {
            _producer.SendToBroker(message, exchangeName);
            return "Complete with message = " + message;
        }
    }
}
