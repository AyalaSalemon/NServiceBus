using Billing.Messages;
using NServiceBus;
using NServiceBus.Logging;
using Receipt.Messages;

namespace Receipts;

public class CreditCardChargedHandler : IHandleMessages<CreditCardCharged>
{
    static ILog log = LogManager.GetLogger<CreditCardChargedHandler>();

    public Task Handle(CreditCardCharged message, IMessageHandlerContext context)
    {
        log.Info($"Received CreditCardCharged, OrderId = {message.OrderId} " +
       $"- Charging credit card...");
        string creditCardNumber = "**** **** **** ";
        for (int i = 4; i > 0; i--)
            creditCardNumber += message.CreditCardNumber[^i];
        log.Info($"Credit Card Number is: {creditCardNumber}");
        log.Info("Publishing Receipt Sent");

        var receiptSent = new ReceiptSent
        {
            ReceiptId = Guid.NewGuid().ToString(),
            Content = "A Receipt"
        };
        return context.Publish(receiptSent);
    }
}
