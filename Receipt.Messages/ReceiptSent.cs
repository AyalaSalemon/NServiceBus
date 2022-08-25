using NServiceBus;

namespace Receipt.Messages;
public class ReceiptSent:IEvent
{
    public string ReceiptId { get; set; }
    public string Content { get; set;}
}
