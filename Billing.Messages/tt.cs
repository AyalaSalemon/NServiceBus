using NServiceBus;

namespace Billing.Messages;
public class tt:ICommand
{
    public string OrderId { get; set; }
    public string CreditCardNumber { get; set; }
}

