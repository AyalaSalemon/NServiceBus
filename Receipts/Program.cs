using NServiceBus;
using System.Data.SqlClient;
class Program
{
    
    public static async Task Main(string[] args)
    {
        Console.Title = "Receipts";

        var endpointConfiguration = new EndpointConfiguration("Receipts");
        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.UseConventionalRoutingTopology(QueueType.Quorum);
        transport.ConnectionString("host = localhost");
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

        while (true)
        {
            try
            {
                var endpointInstance = await Endpoint.Start(endpointConfiguration);

                Console.WriteLine("Press Enter to exit.");
                Console.ReadLine();

                await endpointInstance.Stop();

                break;
            }
            catch (Exception)
            {
                await Task.Delay(5000);
            }
        }
    }

}