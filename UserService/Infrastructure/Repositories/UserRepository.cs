using UserService.Contracts.Providers;
using UserService.Contracts.Repositories;
using UserService.Domain;

namespace UserService.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly Cassandra.ISession _session;
    private readonly Cassandra.Mapping.Mapper _mapper;

    public UserRepository(IDatabaseSessionProvider databaseSessionProvider) : base(databaseSessionProvider)
    {
        _session = databaseSessionProvider.Session;
        _mapper = new Cassandra.Mapping.Mapper(_session);
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _mapper.SingleOrDefaultAsync<User>("WHERE email = ?", email);
    }

    public async Task<User> GetByFirebaseIdAsync(string firebaseId)
    {
        return await _mapper.SingleOrDefaultAsync<User>("WHERE firebase_id = ?", firebaseId);
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        return await _mapper.SingleOrDefaultAsync<User>("WHERE id = ?", id);
    }
}
