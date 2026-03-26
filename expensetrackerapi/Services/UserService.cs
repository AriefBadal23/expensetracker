using expensetrackerapi.Constants;
using expensetrackerapi.Contracts;
using expensetrackerapi.Models;
using Microsoft.AspNetCore.Identity;
using expensetrackerapi.DTO.Auth;
using expensetrackerapi.Results;

namespace expensetrackerapi.Services;

public class UserService:IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    
    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<RegisteredUserDto>> RegisterAsync(RegisterUserDto registerUserDto)
    {
        var user = new ApplicationUser
        {
            Email = registerUserDto.Email,
            FirstName = registerUserDto.FirstName,
            LastName = registerUserDto.LastName,
            UserName = registerUserDto.Email
        };
        var result = await _userManager.CreateAsync(user, registerUserDto.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => new Error(ErrorCodes.BadRequest, e.Description)).ToArray();
            return Result<RegisteredUserDto>.BadRequest(errors);
        }
        
        var registeredUser = new RegisteredUserDto
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Id = user.Id
        };
        
        return Result<RegisteredUserDto>.Success(registeredUser);
    }

    public  async Task<Result<string>> LoginAsync(LoginUserDto loginUserDto)
    {
        var user = await _userManager.FindByEmailAsync(loginUserDto.Email);
        if (user is null)
        {
            return Result<string>.BadRequest(new Error(ErrorCodes.BadRequest, "Invalid credentials"));
        }

        var valid = await _userManager.CheckPasswordAsync(user, loginUserDto.Password);
        
        if (!valid)
        {
            return Result<string>.BadRequest(new Error(ErrorCodes.BadRequest, "Invalid Credentials"));
        }

        return Result<string>.Success("Login successfull.");
    }
}