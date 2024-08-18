using UserService.Contracts.Providers;
using UserService.Contracts.Repositories;
using UserService.Contracts.Services;
using UserService.Domain;

namespace UserService.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IUserProvider userProvider;
    private readonly IUserRepository userRepository;
    private readonly IAuthenticationProviderService authenticationProviderService;

    public UserService(
        IUserProvider userProvider,
        IUserRepository userRepository,
        IAuthenticationProviderService authenticationProviderService)
    {
        this.userProvider = userProvider;
        this.userRepository = userRepository;
        this.authenticationProviderService = authenticationProviderService;
    }

    public async Task<User> GetMeAsync()
    {
        return await userProvider.GetMeAsync();
    }

    public async Task DeleteAsync()
    {
        var user = await userProvider.GetMeAsync();

        await authenticationProviderService.DeleteProvidedUserAsync(user);

        await userRepository.DeleteAsync(user);
    }
}
