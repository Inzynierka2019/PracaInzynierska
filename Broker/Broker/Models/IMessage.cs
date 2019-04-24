namespace Broker.Models
{
    /// <summary>
    /// IMessage interface
    /// </summary>
    public interface IMessage
    {
        long Id { get; set; }

        string Content { get; set; }
    }
}
