using AutoMapper;
using UserService.Contracts.Data;
using UserService.Controllers.Auth.Requests;
using UserService.Controllers.User.Responses;
using UserService.Domain;

namespace UserService.Contracts.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponse>();
        CreateMap<UserSignupInput, User>();
        CreateMap<UserSignupRequest, UserSignupInput>();
    }
}
