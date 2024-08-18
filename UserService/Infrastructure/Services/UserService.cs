using UserService.Contracts.Providers;
using UserService.Contracts.Services;
using UserService.Domain;

namespace UserService.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IUserProvider userProvider;

    public UserService(IUserProvider userProvider)
    {
        this.userProvider = userProvider;
    }

    public async Task<User> GetMeAsync()
    {
        return await userProvider.GetMeAsync();
    }
}
