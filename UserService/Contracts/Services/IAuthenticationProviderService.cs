using UserService.Domain;

namespace UserService.Contracts.Services;

public interface IAuthenticationProviderService
{
    Task UpdateClaimsAsync(User user);
    Task DeleteProvidedUserAsync(User user);
}
