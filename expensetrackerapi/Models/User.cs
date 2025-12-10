
namespace expensetrackerapi.Models;

public enum Role
{
    User,
    Admin
}


public record User
{
    public int Id { get; set; }
    public Role Role { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public DateTime Created_at { get; set; } = DateTime.UtcNow;
}