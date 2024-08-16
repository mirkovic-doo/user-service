using AutoMapper;
using UserService.Controllers.User.Responses;
using UserService.Domain;

namespace UserService.Contracts.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponse>();
    }
}
