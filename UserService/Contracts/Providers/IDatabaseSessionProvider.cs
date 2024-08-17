namespace UserService.Contracts.Providers;

public interface IDatabaseSessionProvider
{
    public Cassandra.ISession Session { get; }
}

