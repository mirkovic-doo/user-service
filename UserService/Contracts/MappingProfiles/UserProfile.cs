using AutoMapper;
using UserService.Contracts.Data;
using UserService.Controllers._Common.Request;
using UserService.Controllers.User.Responses;
using UserService.Domain;

namespace UserService.Contracts.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponse>();
        CreateMap<UserSignupInput, User>();
        CreateMap<UserRequest, UserSignupInput>();
        CreateMap<UserRequest, UserInput>();
        CreateMap<UserInput, User>();
    }
}
