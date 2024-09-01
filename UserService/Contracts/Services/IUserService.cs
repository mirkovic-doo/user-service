using UserService.Contracts.Data;
using UserService.Domain;

namespace UserService.Contracts.Services;

public interface IUserService
{
    Task<User> GetMeAsync();
    Task DeleteAsync();
    Task<User> UpdateAsync(UserInput updateInput);
    Task<User> GetByIdAsync(Guid id);
}
