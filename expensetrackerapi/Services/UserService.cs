using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using expensetrackerapi.Constants;
using expensetrackerapi.Contracts;
using expensetrackerapi.Models;
using Microsoft.AspNetCore.Identity;
using expensetrackerapi.DTO.Auth;
using expensetrackerapi.Results;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace expensetrackerapi.Services;

public class UserService:IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ExpenseTrackerContext _db;
    private readonly IConfiguration _configuration;
    private readonly ILogger<UserService> _logger;
    
    public UserService(UserManager<ApplicationUser> userManager, IConfiguration configuration,ILogger<UserService> logger, ExpenseTrackerContext db)
    {
        _userManager = userManager;
        _configuration = configuration;
        _db = db;
        _logger = logger;
    }
    

    public async Task<Result<RegisteredUserDto>> RegisterAsync(RegisterUserDto registerDto)
    {
        var user = new ApplicationUser
        {
            Email = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            UserName = registerDto.Email
        };
        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => new Error(ErrorCodes.BadRequest, e.Description)).ToArray();
            // TODO: ✅ Input validation failed, we have to log it.
            _logger.LogInformation("Failed to register user");
            
            return Result<RegisteredUserDto>.BadRequest(errors);
        }
        
        var registeredUser = new RegisteredUserDto
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Id = user.Id
        };

        var buckets = _db.Buckets;
        
        var userBuckets = buckets.Select(bucket => new UserBuckets { ApplicationUserId = user.Id, BucketId = bucket.Id }).ToList();
        
        await _db.AddRangeAsync(userBuckets);
        await _db.SaveChangesAsync();
        _logger.LogInformation(
            "User: {UserId} has created an account", user.Id);
        return Result<RegisteredUserDto>.Success(registeredUser);
    }

    public  async Task<Result<string>> LoginAsync(LoginUserDto loginUserDto)
    {
        // TODO: Add logging here for logging success and failed attempts.
        var user = await _userManager.FindByEmailAsync(loginUserDto.Email);
        if (user is null)
        {
            _logger.LogWarning("User tried to login with incorrect credentials");
            return Result<string>.BadRequest(new Error(ErrorCodes.BadRequest, "Invalid credentials"));
        }

        var passwordIsvalid = await _userManager.CheckPasswordAsync(user, loginUserDto.Password);
        
        if (!passwordIsvalid)
        {
            // TODO: Add logging here for logging success and failed attempts.
            _logger.LogWarning("User: {UserId} tried to login with incorrect credentials", user.Id);
            return Result<string>.BadRequest(new Error(ErrorCodes.BadRequest, "Invalid Credentials"));
        }
        
        // Issue a token
        var token = await GenerateToken(user);
        _logger.LogInformation("User: {UserId} signed in successfully", user.Id);
        return Result<string>.Success(token);
    }
    

    private async Task<string> GenerateToken(ApplicationUser user)
    {
      // Set basic user claims
      var claims = new List<Claim>
      {
        new(JwtRegisteredClaimNames.Sub, user.Id),
        new(JwtRegisteredClaimNames.Email, user.Email),
        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new (JwtRegisteredClaimNames.Name,user.FullName )
      };
      // Set user role claims
      var roles = await _userManager.GetRolesAsync(user);
      
      // convert roles to claims
      var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
      // combine all claims in a List
      claims = claims.Union(roleClaims).ToList();
      
      // Set JWT Key credentials
      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
      
      // Hashing of the securityKey by the HmacSha256 algorithm to create a hash (signature)
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
      
      //!! securityKey(secret key) + hashing with the HmacSha256 algorithm = creates the digital signature
      /*
        1. "mySecretKey" (string)
        2. → UTF8 encoding → bytes
        3. → gebruikt als input voor HmacSha256
        4. → genereert signature voor JWT
       
       */
      // Create an encoded token
      var token = new JwtSecurityToken(
          issuer: _configuration["JwtSettings:Issuer"],
          audience: _configuration["JwtSettings:Audience"],
          claims: claims,
          // Universal to avoid unexpected behavior of expiration. UtcNow will avoid this issue
          expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
          signingCredentials:credentials
          
          );
      // Return token value
      return new JwtSecurityTokenHandler().WriteToken(token);
    }
}