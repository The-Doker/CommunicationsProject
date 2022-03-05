namespace CommunicationsProject.Producer
{
    public interface IMessageProducer
    {
        public void SendToBroker(string message, string exchangeName);
    }
}
