namespace expensetrackerapi.DTO.Auth;


// To send the registered user without any sensitive data.
public class RegisteredUserDto
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

}