using expensetrackerapi.Constants;
using expensetrackerapi.Contracts;
using expensetrackerapi.DTO.Auth;
using expensetrackerapi.Results;
using expensetrackerapi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace expensetrackerapi.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _service;

    public AuthController(IUserService service)
    {
        _service = service;

    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterUserDto registerUserDto)
    {
        var result = await _service.RegisterAsync(registerUserDto);
        if (!result.IsSuccess)
        {
            // Errors here is an IEnumerable
            var errors = result.Errors.Select(e => new Error
            (
                ErrorCodes.BadRequest,
                e.Description
            )).ToArray();

            return BadRequest(errors);

        }

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginUserDto loginUserDto)
    {
        // 1. Check if the user exists in the system
        // 2. Check if the user provided the correct password
        var result = await _service.LoginAsync(loginUserDto);
        if (!result.IsSuccess)
        {
            // Errors here is an IEnumerable
            var errors = result.Errors.Select(e => new Error
            (
                ErrorCodes.BadRequest,
                e.Description
            )).ToArray();

            return BadRequest(errors);
        }
        return Ok(result);
    }

}
