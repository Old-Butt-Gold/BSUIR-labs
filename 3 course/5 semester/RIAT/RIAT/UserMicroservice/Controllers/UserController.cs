using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserMicroservice.Context;
using UserMicroservice.Models;
using UserMicroservice.Models.DTO;

namespace UserMicroservice.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UsersController : ControllerBase
{
    readonly ApplicationContext _context;
    readonly PasswordHasher<User> _passwordHasher;
    readonly IConfiguration _configuration;
    private readonly ILogger<UsersController> _logger;

    public UsersController(ApplicationContext context, IConfiguration configuration, ILogger<UsersController> logger)
    {
        _context = context;
        _passwordHasher = new PasswordHasher<User>();
        _configuration = configuration;
        _logger = logger;
    }

    void LogRequest()
    {
        var request = HttpContext.Request;
        var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP";
        var requestInfo = $"Method: {request.Method}, Path: {request.Path}, QueryString: {request.QueryString}, IP: {clientIp}";
        _logger.LogInformation("Request Info: {RequestInfo}", requestInfo);
    }
    
    /// <summary>
    /// Проверяет токен и возвращает ID пользователя.
    /// </summary>
    private Guid? ValidateTokenAndGetUserId(string token)
    {
        _logger.LogInformation("Validating token...");
        var jwtKey = _configuration["JwtSettings:Key"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!));
        var audience = _configuration["JwtSettings:Audience"];
        var issuer = _configuration["JwtSettings:Issuer"];
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
            }, out SecurityToken validatedToken);
            
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "UserId");
            return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating token.");
            return null;
        }
    }

    /// <summary>
    /// Get user by ID including profile settings.
    /// </summary>
    /// <param name="id">User ID.</param>
    /// <returns>User with profile settings.</returns>
    /// <response code="200">Returns the user data including profile settings.</response>
    /// <response code="404">If the user is not found.</response>
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(Guid id)
    {
        LogRequest();
        _logger.LogInformation("Fetching user with ID: {UserId}", id);
        
        var user = await _context.Users
            .Include(u => u.ProfileSetting)
            .FirstOrDefaultAsync(u => u.UserId == id);

        if (user == null)
        {
            _logger.LogWarning("User with ID: {UserId} not found.", id);
            return NotFound();
        }

        _logger.LogInformation("User with ID: {UserId} retrieved successfully.", id);
        return user;
    }
    
    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="userRegistrationDto">User data transfer object containing email and plain text password.</param>
    /// <returns>Created user.</returns>
    /// <response code="201">Returns the newly created user</response>
    /// <response code="400">If the email or password is missing</response>
    /// <response code="409">If a user with the same email already exists</response>
    [AllowAnonymous]
    [HttpPost("create")]
    public async Task<ActionResult<User>> CreateUser([FromBody] UserRegistrationDto userRegistrationDto)
    {
        LogRequest();
        if (string.IsNullOrEmpty(userRegistrationDto.Email) || string.IsNullOrEmpty(userRegistrationDto.Password) || string.IsNullOrEmpty(userRegistrationDto.Username))
        {
            _logger.LogWarning("Invalid registration attempt. Email or password missing.");
            return BadRequest("Email and password are required");
        }
        
        _logger.LogInformation("Checking if user with email {Email} or username {Username} already exists.", userRegistrationDto.Email, userRegistrationDto.Username);

        // Check if the user already exists
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userRegistrationDto.Email);
        if (existingUser != null)
        {
            _logger.LogWarning("User with email {Email} already exists.", userRegistrationDto.Email);
            return Conflict("User with this email already exists");
        }

        existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == userRegistrationDto.Username);
        if (existingUser != null)
        {
            _logger.LogWarning("User with username {Username} already exists.", userRegistrationDto.Username);
            return Conflict("User with this username already exists");
        }
        
        _logger.LogInformation("Creating new user with email {Email}.", userRegistrationDto.Email);

        // Create new user and hash the password
        var guid = Guid.NewGuid();
        var user = new User
        {
            UserId = guid,
            Email = userRegistrationDto.Email,
            Username = userRegistrationDto.Username,
            ProfileSetting = new ProfileSetting { UserId = guid },
            CreatedAt = DateTime.UtcNow,
        };

        // Hash the password using PasswordHasher
        user.PasswordHash = _passwordHasher.HashPassword(user, userRegistrationDto.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        _logger.LogInformation("User with email {Email} created successfully.", userRegistrationDto.Email);
        return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
    }

    
    
    /// <summary>
    /// User login with either Email/Password or Username/Password.
    /// </summary>
    /// <param name="userLoginDto">User data transfer object containing either username or email, and plain text password.</param>
    /// <returns>Login status with JWT token.</returns>
    /// <response code="200">Returns the JWT token.</response>
    /// <response code="401">If the login credentials are invalid.</response>
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<User>> Login([FromBody] UserLoginDto userLoginDto)
    {
        LogRequest();
        if (string.IsNullOrEmpty(userLoginDto.UsernameOrEmail) || string.IsNullOrEmpty(userLoginDto.Password))
        {
            _logger.LogWarning("Login attempt with missing credentials.");
            return BadRequest("Username or email and password are required");
        }

        _logger.LogInformation("Attempting to find user with email or username {UsernameOrEmail}.", userLoginDto.UsernameOrEmail);

        var user = await _context.Users.FirstOrDefaultAsync(u =>
            u.Email == userLoginDto.UsernameOrEmail || u.Username == userLoginDto.UsernameOrEmail);

        if (user == null)
        {
            _logger.LogWarning("User with email or username {UsernameOrEmail} not found.", userLoginDto.UsernameOrEmail);
            return NotFound("Invalid email or password");
        }

        var passwordVerificationResult =
            _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userLoginDto.Password);
        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            _logger.LogWarning("Invalid password for user {UsernameOrEmail}.", userLoginDto.UsernameOrEmail);
            return Unauthorized("Invalid email or password.");
        }

        user.Status = "Active";
        user.LastLogin = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        var token = CreateToken(user);
        _logger.LogInformation("User {UsernameOrEmail} logged in successfully.", userLoginDto.UsernameOrEmail);
        return Ok(new { Token = "Bearer " + token });

        string CreateToken(User user)
        {
            // Reading JWT settings from configuration
            var jwtKey = _configuration["JwtSettings:Key"];
            var jwtIssuer = _configuration["JwtSettings:Issuer"];
            var jwtAudience = _configuration["JwtSettings:Audience"];
            var durationInMinutes = Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"]);

            var claims = new List<Claim>
            {
                new Claim("UserId", user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!));
            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(durationInMinutes),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    /// <summary>
    /// Logout a user and mark them as inactive.
    /// </summary>
    /// <returns>Logout status message.</returns>
    /// <response code="200">Returns a success message indicating the user has been logged out.</response>
    /// <response code="401">If the user is not authenticated or the token is invalid.</response>
    /// <response code="404">If the user is not found.</response>
    [HttpPost("exit")]
    public async Task<IActionResult> Logout()
    {
        LogRequest();
        _logger.LogInformation("Logout attempt initiated.");

        var authHeader = Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            _logger.LogWarning("Logout attempt failed: JWT token is missing or invalid.");
            return Unauthorized("JWT token is missing or invalid in the request header.");
        }

        var token = authHeader["Bearer ".Length..].Trim();
        var id = ValidateTokenAndGetUserId(token);
        if (id == null)
        {
            _logger.LogWarning("Logout attempt failed: Invalid or expired token.");
            return Unauthorized("Invalid or expired token.");
        }

        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            _logger.LogWarning("Logout attempt failed: User with ID {UserId} not found.", id);
            return NotFound("User not found");
        }

        user.Status = "Inactive";
        await _context.SaveChangesAsync();
        _logger.LogInformation("User with ID {UserId} logged out successfully.", id);

        return Ok("User logged out");
    }

    /// <summary>
    /// Update the current user's profile settings.
    /// </summary>
    /// <param name="userProfileUpdateDto">The user profile update data transfer object.</param>
    /// <returns>Updated user profile settings.</returns>
    /// <response code="200">Returns the updated user data.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="404">If the user is not found.</response>
    [HttpPatch("me/settings")]
    public async Task<IActionResult> PatchProfileSettings([FromBody] UserProfileUpdateDto userProfileUpdateDto)
    {
        LogRequest();
        _logger.LogInformation("Profile settings update attempt started.");

        var authHeader = Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            _logger.LogWarning("Profile settings update failed: JWT token is missing or invalid.");
            return Unauthorized("JWT token is missing or invalid in the request header.");
        }

        var token = authHeader["Bearer ".Length..].Trim();
        var userId = ValidateTokenAndGetUserId(token);
        if (userId == null)
        {
            _logger.LogWarning("Profile settings update failed: Invalid or expired token.");
            return Unauthorized("Invalid or expired token.");
        }

        var user = await _context.Users.Include(u => u.ProfileSetting)
            .FirstOrDefaultAsync(u => u.UserId == userId.Value);

        if (user == null)
        {
            _logger.LogWarning("Profile settings update failed: User with ID {UserId} not found.", userId);
            return NotFound("User not found.");
        }

        if (!string.IsNullOrEmpty(userProfileUpdateDto.Username))
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == userProfileUpdateDto.Username && u.UserId != userId);

            if (existingUser != null)
            {
                _logger.LogWarning("Profile settings update failed: Username {Username} is already taken.", userProfileUpdateDto.Username);
                return Conflict("Username is already taken.");
            }

            user.Username = userProfileUpdateDto.Username;
        }

        if (!string.IsNullOrEmpty(userProfileUpdateDto.ProfilePicture))
        {
            user.ProfilePicture = userProfileUpdateDto.ProfilePicture;
        }

        if (user.ProfileSetting != null)
        {
            user.ProfileSetting.Theme = userProfileUpdateDto.Theme ?? user.ProfileSetting.Theme;
            user.ProfileSetting.Language = userProfileUpdateDto.Language ?? user.ProfileSetting.Language;
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Profile settings for user with ID {UserId} updated successfully.", userId);

        return Ok(user);
    }
    
    /// <summary>
    /// Retrieves the current authenticated user profile using JWT.
    /// </summary>
    /// <returns>Current authenticated user.</returns>
    /// <response code="200">Returns the current user's information</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="404">If the user is not found</response>
    [HttpGet("me")]
    public async Task<ActionResult<User>> GetCurrentUser()
    {
        LogRequest();
        _logger.LogInformation("Retrieving current user profile.");

        var authHeader = Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            _logger.LogWarning("Failed to retrieve user profile: JWT token is missing or invalid.");
            return Unauthorized("JWT token is missing or invalid in the request header.");
        }

        var token = authHeader["Bearer ".Length..].Trim();
        var userId = ValidateTokenAndGetUserId(token);
        if (userId == null)
        {
            _logger.LogWarning("Failed to retrieve user profile: Invalid or expired token.");
            return Unauthorized("Invalid or expired token.");
        }

        var user = await _context.Users
            .Include(u => u.ProfileSetting)
            .FirstOrDefaultAsync(u => u.UserId == userId.Value);

        if (user == null)
        {
            _logger.LogWarning("User profile retrieval failed: User with ID {UserId} not found.", userId);
            return NotFound("User not found.");
        }

        _logger.LogInformation("User profile for user with ID {UserId} retrieved successfully.", userId);
        return Ok(user);
    }
}