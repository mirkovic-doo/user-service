using UserService.Contracts.Constants;
using UserService.Contracts.Providers;
using UserService.Contracts.Repositories;
using UserService.Domain;

namespace UserService.Infrastructure.Provider;

public class UserProvider : IUserProvider
{
    private readonly IUserRepository userRepository;
    private readonly IHttpContextAccessor httpContextAccessor;
    private User currentUser;

    public UserProvider(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
    {
        this.userRepository = userRepository;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<User> GetMeAsync()
    {
        if (currentUser == null)
        {
            var identityClaim = httpContextAccessor.HttpContext?.User.FindFirst(CustomClaims.FirebaseId);
            var id = identityClaim?.Value;

            if (id == null)
            {
                throw new Exception("User not authenticated");
            }

            var user = await userRepository.GetByFirebaseIdAsync(id);

            currentUser = user ?? throw new Exception("User not found");
            return user;
        }

        return currentUser;
    }
}
