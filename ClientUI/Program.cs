using Billing.Messages;
using NServiceBus;
using NServiceBus.Logging;
public class Program
{
    static async Task Main()
    {
        Console.Title = "ClientUI";

        var endpointConfiguration = new EndpointConfiguration("ClientUI");
        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.UseConventionalRoutingTopology(QueueType.Quorum);
        transport.ConnectionString("host = localhost;");
        var routing = transport.Routing();
        routing.RouteToEndpoint(typeof(ChargeCreditCard).Assembly, "Billing");
        endpointConfiguration.SendOnly();
        var endpointInstance = await Endpoint.Start(endpointConfiguration);
      
        await RunLoop(endpointInstance);
       
        await endpointInstance.Stop();

      

    }
    static ILog log = LogManager.GetLogger<Program>();
    static async Task RunLoop(IEndpointInstance endpointInstance)
    {
        while (true)
        {
            log.Info("Press 'C' to charge your card, or 'Q' to quit.");
            var key = Console.ReadKey();
            Console.WriteLine();
            switch (key.Key)
            {
                case ConsoleKey.C:

                    Console.WriteLine("Enter credit card number");
                    string CreditCardNumber = Console.ReadLine();
                    bool status = false;
                    DateTime expiring = new DateTime();
                    while (!status)
                    {
                        Console.WriteLine("Enter The Card Expire Date");
                        string ExpireDate = Console.ReadLine();
                        status = DateTime.TryParse(ExpireDate, out expiring) ? true : false;
                        if (status == false)
                            Console.WriteLine("The date you entered is not valid!");
                    }
                    var command = new ChargeCreditCard
                    {
                        OrderId = Guid.NewGuid().ToString(),
                        CreditCardNumber = CreditCardNumber,
                        ExpireDate = expiring

                    };
                   
                    log.Info($"Sending ChargeCreditCard command, OrderId = {command.OrderId}");
                    await endpointInstance.Send(command);
                    break;
                case ConsoleKey.Q:
                    return;
                default:
                    log.Info("Unknown input. Please try again.");
                    break;
            }
        }
    }
}
