using UserService.Contracts.Data;
using UserService.Domain;

namespace UserService.Contracts.Services;

public interface IAuthService
{
    Task<User> SignupAsync(UserSignupInput input);
}
