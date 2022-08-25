using NServiceBus;
using NServiceBus.Logging;
using Receipt.Messages;

namespace Shipping;

internal class ReceiptSentHandler : IHandleMessages<ReceiptSent>
{
    static ILog log = LogManager.GetLogger<ReceiptSentHandler>();
    public Task Handle(ReceiptSent message, IMessageHandlerContext context)
    {
        log.Info($"Received Receipt Sent, ReceiptId = {message.ReceiptId} , Content = {message.Content} - Shipping the stuff...");

        return Task.CompletedTask;
    }
}
