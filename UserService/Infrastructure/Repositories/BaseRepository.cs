using UserService.Contracts.Providers;
using UserService.Contracts.Repositories;

namespace UserService.Infrastructure.Repositories;

public class BaseRepository<T> : IRepository<T> where T : class
{
    private readonly Cassandra.ISession session;
    private readonly Cassandra.Mapping.Mapper mapper;

    public BaseRepository(IDatabaseSessionProvider databaseSessionProvider)
    {
        session = databaseSessionProvider.Session;
        mapper = new Cassandra.Mapping.Mapper(session);
    }

    public async Task<T> CreateAsync(T entity)
    {
        await mapper.InsertAsync(entity);
        return entity;
    }

    public async Task DeleteAsync(T entity)
    {
        await mapper.DeleteAsync(entity);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return (await mapper.FetchAsync<T>()).ToList();
    }

    public async Task<T> UpdateAsync(T entity)
    {
        await mapper.UpdateAsync(entity);
        return entity;
    }
}
