using NServiceBus;

namespace Billing.Messages;
public class CreditCardCharged: IEvent
{
    public string OrderId { get; set; }
    public string CreditCardNumber { get; set; }
}
