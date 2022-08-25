using Billing;
using NServiceBus;
using NServiceBus.Logging;
using System.Data.SqlClient;
class Program
{
    static async Task Main()
    {
        Console.Title = "Billing";       
        var endpointConfiguration = new EndpointConfiguration("Billing");
        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.UseConventionalRoutingTopology(QueueType.Quorum);
        transport.ConnectionString("host = localhost");
        var recoverability = endpointConfiguration.Recoverability();
        recoverability.Immediate(
            customizations: imeddiate =>
            imeddiate.NumberOfRetries(1)
            );
        recoverability.Delayed(
            customizations: delayed =>
            {
                delayed.NumberOfRetries(1);
            });
        
        var defaultFactory = LogManager.Use<DefaultFactory>();
        defaultFactory.Level(LogLevel.Info);

        var connection = "Server=localhost;database=LearningNServiceBus;Trusted_Connection=True;";
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var subscriptions = persistence.SubscriptionSettings();
        subscriptions.CacheFor(TimeSpan.FromMinutes(1));       
        persistence.ConnectionBuilder(
            connectionBuilder: () =>
            {
                return new SqlConnection(connection);
            });
        var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
        dialect.Schema("NSB");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.EnableOutbox();
        endpointConfiguration.AuditProcessedMessagesTo("audit");
        endpointConfiguration.SendFailedMessagesTo("error");
        SubscribeToNotifications.Subscribe(endpointConfiguration);
        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Please press enter to exit....");
        Console.ReadLine();
        await endpointInstance.Stop();
       
    
    }
  
}