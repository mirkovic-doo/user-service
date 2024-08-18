using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Contracts.Data;
using UserService.Contracts.Services;
using UserService.Controllers._Common.Request;
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

    [HttpPut(Name = nameof(UpdateUser))]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateUser([FromBody] UserRequest request)
    {
        var updateInput = mapper.Map<UserInput>(request);
        var user = await userService.UpdateAsync(updateInput);

        return Ok(mapper.Map<UserResponse>(user));
    }

    [HttpDelete(Name = nameof(DeleteUser))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteUser()
    {
        await userService.DeleteAsync();
        return Ok("User deleted successfully");
    }

    [AllowAnonymous]
    [HttpGet("email/{username}", Name = nameof(GetEmailByUsername))]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEmailByUsername([FromRoute] string username)
    {
        return Ok(await userService.GetUserEmailByUsernameAsync(username));
    }
}
