using Cassandra;
using Microsoft.Extensions.Options;
using UserService.Configuration;
using UserService.Contracts.Providers;

namespace UserService.Infrastructure.Provider;

public sealed class DatabaseSessionProvider : IDatabaseSessionProvider
{
    private Cassandra.ISession session;
    private readonly DatabaseConfig databaseConfig;

    public DatabaseSessionProvider(IOptions<DatabaseConfig> databaseConfigOptions)
    {
        databaseConfig = databaseConfigOptions.Value;
        session = InitializeSession();
    }

    private Cassandra.ISession InitializeSession()
    {
        return Cluster.Builder()
            .WithCloudSecureConnectionBundle(databaseConfig.DatabaseSecureConnectPath)
            .WithDefaultKeyspace(databaseConfig.DefaultKeyspace)
            .WithCredentials(databaseConfig.ClientId, databaseConfig.ClientSecret)
            .WithReconnectionPolicy(new ConstantReconnectionPolicy(1000))
            .WithLoadBalancingPolicy(new TokenAwarePolicy(Policies.DefaultPolicies.LoadBalancingPolicy))
            .Build()
            .Connect();
    }

    public Cassandra.ISession Session
    {
        get
        {
            session ??= InitializeSession();

            return session;
        }
    }
}