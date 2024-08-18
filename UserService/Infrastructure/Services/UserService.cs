using AutoMapper;
using UserService.Contracts.Data;
using UserService.Contracts.Providers;
using UserService.Contracts.Repositories;
using UserService.Contracts.Services;
using UserService.Domain;

namespace UserService.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IMapper mapper;
    private readonly IUserProvider userProvider;
    private readonly IUserRepository userRepository;
    private readonly IAuthenticationProviderService authenticationProviderService;

    public UserService(
        IMapper mapper,
        IUserProvider userProvider,
        IUserRepository userRepository,
        IAuthenticationProviderService authenticationProviderService)
    {
        this.mapper = mapper;
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

    public async Task<User> UpdateAsync(UserInput updateInput)
    {
        var user = await userProvider.GetMeAsync();

        var userWithUsername = await userRepository.GetByUsernameAsync(updateInput.Username);

        if (userWithUsername != null && userWithUsername.Id != user.Id)
        {
            throw new Exception("Username already exists");
        }

        mapper.Map(updateInput, user);

        await userRepository.UpdateAsync(user);

        return user;
    }

    public async Task<string> GetUserEmailByUsernameAsync(string username)
    {
        var user = await userRepository.GetByUsernameAsync(username);

        return user?.Email ?? string.Empty;
    }
}
