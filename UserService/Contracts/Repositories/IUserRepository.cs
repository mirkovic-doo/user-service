using UserService.Domain;

namespace UserService.Contracts.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetByFirebaseIdAsync(string firebaseId);
    Task<User> GetByEmailAsync(string email);
    Task<User> GetByUsernameAsync(string username);
}
