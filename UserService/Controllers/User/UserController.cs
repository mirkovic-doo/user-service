using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Contracts.Services;
using UserService.Controllers.User.Responses;

namespace UserService.Controllers.User;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
public class UserController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IUserService userService;

    public UserController(
        IMapper mapper,
        IUserService userService)
    {
        this.mapper = mapper;
        this.userService = userService;
    }

    [HttpGet("me", Name = nameof(GetMe))]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMe()
    {
        return Ok(mapper.Map<UserResponse>(await userService.GetMeAsync()));
    }
}
