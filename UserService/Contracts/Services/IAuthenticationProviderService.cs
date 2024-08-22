using UserService.Domain;

namespace UserService.Contracts.Services;

public interface IAuthenticationProviderService
{
    Task UpdateClaimsAsync(User user);
    Task DeleteProvidedUserAsync(User user);
    Task UpdateEmailAsync(User user, string newEmail);
}
