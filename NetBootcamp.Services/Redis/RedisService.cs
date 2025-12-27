using StackExchange.Redis;

namespace NetBootcamp.Services.Redis;

public class RedisService
{
    public IDatabase Database;

    public RedisService(string connectionString)
    {
        var connectionMultiplexer = ConnectionMultiplexer.Connect("localhost:6379");

        // connection failed event
        connectionMultiplexer.ConnectionFailed += (sender, args) =>
        {
            // Log the connection failure details
            Console.WriteLine($"Redis connection failed: {args.Exception.Message}");
        };


        Database = connectionMultiplexer.GetDatabase(1);
    }
}
