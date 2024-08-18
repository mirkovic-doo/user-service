using UserService.Domain;

namespace UserService.Contracts.Providers;

public interface IUserProvider
{
    Task<User> GetMeAsync();
}
