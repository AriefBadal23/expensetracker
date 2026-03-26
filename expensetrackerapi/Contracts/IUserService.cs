using expensetrackerapi.DTO.Auth;
using expensetrackerapi.Results;

namespace expensetrackerapi.Contracts;

public interface IUserService
{
    public Task<Result<RegisteredUserDto>> RegisterAsync(RegisterUserDto registerDto);

    public Task<Result<string>> LoginAsync(LoginUserDto loginUserDto);

}