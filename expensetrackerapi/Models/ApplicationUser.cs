using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace expensetrackerapi.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    [NotMapped] // EF Core will ignore this property
    public string FullName => $"{LastName}, {FirstName}";

    public List<Bucket> Buckets { get; } = [];
    public List<UserBuckets> UserBuckets { get; } = [];
    
    
}