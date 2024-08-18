using UserService.Domain;

namespace UserService.Contracts.Services;

public interface IUserService
{
    Task<User> GetMeAsync();
}
