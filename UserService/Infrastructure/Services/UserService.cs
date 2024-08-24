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

        var userWithEmail = await userRepository.GetByEmailAsync(updateInput.Email);

        if (userWithEmail != null && userWithEmail.Id != user.Id)
        {
            throw new Exception("Email already exists");
        }

        if (user.Email != updateInput.Email)
        {
            await authenticationProviderService.UpdateEmailAsync(user, updateInput.Email);
        }

        mapper.Map(updateInput, user);

        await userRepository.UpdateAsync(user);

        return user;
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        var user = await userRepository.GetByIdAsync(id);

        if (user == null)
        {
            throw new Exception("User not found");
        }

        return user;
    }
}
