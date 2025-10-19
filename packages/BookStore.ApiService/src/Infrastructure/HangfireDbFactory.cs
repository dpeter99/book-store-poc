using Npgsql;

namespace BookStore.ApiService.Infrastructure;

public class HangfireDbFactory(
    //IServiceProvider sp,
    Func<string> getConnectionString
) : Hangfire.PostgreSql.IConnectionFactory
{
    
        
        
    public NpgsqlConnection GetOrCreateConnection()
    {
        NpgsqlDataSourceBuilder dataSourceBuilder = new NpgsqlDataSourceBuilder(getConnectionString());

        var dataSource = dataSourceBuilder.Build();

        return dataSource.CreateConnection();
    }
}