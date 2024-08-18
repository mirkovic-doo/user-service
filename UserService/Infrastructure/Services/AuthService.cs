using AutoMapper;
using UserService.Contracts.Data;
using UserService.Contracts.Repositories;
using UserService.Contracts.Services;
using UserService.Domain;

namespace UserService.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IMapper mapper;
    private readonly IUserRepository userRepository;
    private readonly IAuthenticationProviderService authenticationProviderService;

    public AuthService(
        IMapper mapper,
        IUserRepository userRepository,
        IAuthenticationProviderService authenticationProviderService)
    {
        this.mapper = mapper;
        this.userRepository = userRepository;
        this.authenticationProviderService = authenticationProviderService;
    }

    public async Task<User> SignupAsync(UserSignupInput input)
    {
        var existingUser = await userRepository.GetByFirebaseIdAsync(input.FirebaseId);

        if (existingUser != null && existingUser.IsGuest != input.IsGuest)
        {
            throw new Exception($"User already registered as {(existingUser.IsGuest ? "guest" : "host")}");
        }

        if (existingUser != null)
        {
            return existingUser;
        }

        var userWithUsername = await userRepository.GetByUsernameAsync(input.Username);

        if (userWithUsername != null && userWithUsername.FirebaseId != input.FirebaseId)
        {
            throw new Exception("Username already exists");
        }

        var user = mapper.Map<User>(input);
        user.Id = Guid.NewGuid();

        await userRepository.CreateAsync(user);

        await authenticationProviderService.UpdateClaimsAsync(user);

        return user;
    }
}
