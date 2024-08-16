using UserService.Contracts.Services;
using UserService.Domain;

namespace UserService.Infrastructure.Services;

public class UserService : IUserService
{
    public UserService()
    {

    }

    public async Task<User> ReadMockUser()
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Username = "test123",
            FirstName = "Test",
            LastName = "Last",
            Email = "test@test.com",
            Country = "US",
            City = "MockUpCity",
            Street = "Main Street",
            HouseNumber = "1",
            PostCode = "21000"
        };
    }
}
