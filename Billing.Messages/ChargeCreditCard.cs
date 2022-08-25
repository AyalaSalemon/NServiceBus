using NServiceBus;


namespace Billing.Messages;
public class ChargeCreditCard :ICommand
{
    public string OrderId { get; set; }
    public string CreditCardNumber { get; set; }
    public DateTime? ExpireDate { get; set; }
}
