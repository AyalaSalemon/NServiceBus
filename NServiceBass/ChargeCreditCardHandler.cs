using Billing.Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace Billing;

internal class ChargeCreditCardHandler : IHandleMessages<ChargeCreditCard>,
    IHandleMessages<tt>
{
    static ILog log = LogManager.GetLogger<ChargeCreditCardHandler>();
    public Task Handle(tt message, IMessageHandlerContext context)
    {
        log.Info("tt received");
        return Task.CompletedTask;
    }
        public Task Handle(ChargeCreditCard message, IMessageHandlerContext context)
    {
        log.Info($"Received chargeCreditCard");
        string creditCardNumber = "**** **** **** ";
        for (int i = 4; i > 0; i--)
            creditCardNumber += message.CreditCardNumber[^i];
        log.Info($"Credit Card Number is: {creditCardNumber}");
        log.Info("Publishing creditCardCharged");
        var creditCardCharged = new CreditCardCharged
        {
            OrderId = message.OrderId,
            CreditCardNumber= message.CreditCardNumber
        };
        return context.Publish(creditCardCharged);
    }
}
