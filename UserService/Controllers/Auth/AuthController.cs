using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserService.Contracts.Constants;
using UserService.Contracts.Data;
using UserService.Contracts.Services;
using UserService.Controllers.Auth.Requests;
using UserService.Controllers.User.Responses;

namespace UserService.Controllers.Auth;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
public class AuthController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly IAuthService authService;

    public AuthController(IMapper mapper, IAuthService authService)
    {
        this.mapper = mapper;
        this.authService = authService;
    }

    [HttpPost("signup/guest", Name = nameof(SignupGuest))]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SignupGuest([FromBody] UserSignupRequest request)
    {
        var userInput = GetUserSignupInput(request, true);

        var user = await authService.SignupAsync(userInput);
        return Ok(mapper.Map<UserResponse>(user));
    }

    [HttpPost("signup/host", Name = nameof(SignupHost))]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> SignupHost([FromBody] UserSignupRequest request)
    {
        var userInput = GetUserSignupInput(request, false);

        var user = await authService.SignupAsync(userInput);
        return Ok(mapper.Map<UserResponse>(user));
    }

    private UserSignupInput GetUserSignupInput(UserSignupRequest request, bool isGuest)
    {
        var userInput = mapper.Map<UserSignupInput>(request);
        userInput.FirebaseId = HttpContext.User.FindFirstValue(CustomClaims.FirebaseId) ?? throw new Exception("Email not found");
        userInput.Email = HttpContext.User.FindFirstValue(General.FirebaseEmailSchema) ?? throw new Exception("Email not found");
        userInput.IsGuest = isGuest;

        return userInput;
    }
}
