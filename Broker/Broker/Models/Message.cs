namespace Broker.Models
{
    /// <summary>
    /// Message class
    /// </summary>
    public class Message : IMessage
    {
        public long Id { get; set; }

        public string Content { get; set; }
    }
}
