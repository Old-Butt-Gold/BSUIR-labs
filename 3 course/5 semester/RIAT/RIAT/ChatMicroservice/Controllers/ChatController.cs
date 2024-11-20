using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ChatMicroservice.Context;
using ChatMicroservice.Models;
using ChatMicroservice.Models.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ChatMicroservice.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ChatController : ControllerBase
{
    readonly ApplicationContext _context;
    readonly IConfiguration _configuration;
    readonly UserIdProducer _userIdProducer;
    private readonly ILogger<ChatController> _logger;

    public ChatController(ApplicationContext context, IConfiguration configuration, UserIdProducer userIdProducer, ILogger<ChatController> logger)
    {
        _context = context;
        _configuration = configuration;
        _userIdProducer = userIdProducer;
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
    Guid? ValidateTokenAndGetUserId(string token)
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
            _logger.LogInformation("Token validation successful.");
            return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating token.");
            return null;
        }
    }

    /// <summary>
    /// Получить список чатов, в которых состоит пользователь.
    /// </summary>
    [HttpGet("my-chats")]
    public async Task<IActionResult> GetMyChats()
    {
        LogRequest();
        _logger.LogInformation("Getting user's chats...");

        var authHeader = Request.Headers.Authorization.ToString();
        var token = authHeader["Bearer ".Length..].Trim();
        var userId = ValidateTokenAndGetUserId(token);

        if (userId == null)
        {
            _logger.LogWarning("Invalid or expired token.");
            return Unauthorized("Invalid or expired token.");
        }

        var chats = await _context.ChatMembers
            .Where(cm => cm.UserId == userId)
            .Include(cm => cm.Chat)
            .ThenInclude(chat => chat.ChatMembers)
            .Select(cm => cm.Chat)
            .ToListAsync();

        _logger.LogInformation("Retrieved {ChatCount} chats for user.", chats.Count);
        return Ok(chats);
    }

    /// <summary>
    /// Создает новый чат.
    /// </summary>
    [HttpPost("create")]
    public async Task<IActionResult> CreateChat([FromBody] ChatCreateDto chatDto)
    {
        LogRequest();
        _logger.LogInformation("Creating a new chat...");

        if (string.IsNullOrEmpty(chatDto.ChatName))
        {
            _logger.LogWarning("ChatName is missing or empty.");
            return BadRequest("ChatName is required.");
        }

        var authHeader = Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            _logger.LogWarning("JWT token is missing or invalid in the request header.");
            return Unauthorized("JWT token is missing or invalid in the request header.");
        }

        var token = authHeader["Bearer ".Length..].Trim();
        var userId = ValidateTokenAndGetUserId(token);
        if (userId == null)
        {
            _logger.LogWarning("Invalid or expired token.");
            return Unauthorized("Invalid or expired token.");
        }

        var newChat = new Chat
        {
            ChatId = Guid.NewGuid(),
            ChatName = chatDto.ChatName,
            IsGroup = chatDto.IsGroup,
            CreatedAt = DateTime.UtcNow
        };

        _context.Chats.Add(newChat);

        var chatMember = new ChatMember
        {
            ChatId = newChat.ChatId,
            UserId = userId,
            JoinedAt = DateTime.UtcNow,
            NotificationsEnabled = true
        };

        _context.ChatMembers.Add(chatMember);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Chat created successfully with ID: {ChatId}", newChat.ChatId);
        return Ok(newChat);
    }

    /// <summary>
    /// Поиск чатов по параметру Search.
    /// </summary>
    [HttpPost("search")]
    public async Task<IActionResult> SearchChats([FromBody] string search)
    {
        LogRequest();
        _logger.LogInformation("Searching for chats with keyword: {Search}", search);

        if (string.IsNullOrEmpty(search))
        {
            _logger.LogWarning("Search parameter is missing or empty.");
            return BadRequest("Search parameter is required.");
        }

        var chats = await _context.Chats
            .Where(c => c.ChatName != null && c.ChatName.Contains(search))
            .Include(chat => chat.ChatMembers)
            .ToListAsync();

        _logger.LogInformation("Found {ChatCount} chats matching the search.", chats.Count);
        return Ok(chats);
    }

    /// <summary>
    /// Получить групповые/не групповые чаты.
    /// </summary>
    [HttpGet("group-chats")]
    public async Task<IActionResult> GetGroupChats(bool isGroup)
    {
        LogRequest();
        _logger.LogInformation("Getting {Type} chats.", isGroup ? "group" : "non-group");

        var chats = await _context.Chats
            .Where(c => c.IsGroup == isGroup)
            .Include(chat => chat.ChatMembers)
            .ToListAsync();

        _logger.LogInformation("Retrieved {ChatCount} {Type} chats.", chats.Count, isGroup ? "group" : "non-group");
        return Ok(chats);
    }

    /// <summary>
    /// Добавляет участника в чат по ChatId и UserName участника.
    /// </summary>
    [HttpPost("add-member")]
    public async Task<IActionResult> AddMemberToChat([FromBody] AddMemberDto addMemberDto)
    {
        LogRequest();
        _logger.LogInformation("Adding member {Username} to chat {ChatId}", addMemberDto.Username, addMemberDto.ChatId);

        if (string.IsNullOrEmpty(addMemberDto.Username) || addMemberDto.ChatId == Guid.Empty)
        {
            _logger.LogWarning("Username or ChatId is missing.");
            return BadRequest("Username and ChatId are required.");
        }

        var userIdFromUsername = await _userIdProducer.GetUserIdByUsernameAsync(addMemberDto.Username);
        if (userIdFromUsername == null)
        {
            _logger.LogWarning("User not found: {Username}", addMemberDto.Username);
            return NotFound("User not found.");
        }

        var chat = await _context.Chats.Include(c => c.ChatMembers)
            .FirstOrDefaultAsync(c => c.ChatId == addMemberDto.ChatId);
        if (chat == null)
        {
            _logger.LogWarning("Chat not found: {ChatId}", addMemberDto.ChatId);
            return NotFound("Chat not found.");
        }

        if (chat.ChatMembers.Any(cm => cm.UserId == userIdFromUsername.Value))
        {
            _logger.LogWarning("User {Username} is already a member of chat {ChatId}.", addMemberDto.Username, addMemberDto.ChatId);
            return BadRequest("This user is already a member of the chat.");
        }

        if (!chat.IsGroup.GetValueOrDefault() && chat.ChatMembers.Count >= 2)
        {
            _logger.LogWarning("Non-group chat {ChatId} already has 2 members.", addMemberDto.ChatId);
            return BadRequest("Only 2 members can be added to a non-group chat.");
        }

        
        int max = _context.ChatMembers.Max(x => x.ChatMemberId);
        var chatMember = new ChatMember
        {
            ChatMemberId = max + 1,
            ChatId = chat.ChatId,
            UserId = userIdFromUsername.Value,
            JoinedAt = DateTime.UtcNow,
            NotificationsEnabled = true
        };

        _context.ChatMembers.Add(chatMember);
        await _context.SaveChangesAsync();

        _logger.LogInformation("User {Username} added to chat {ChatId} successfully.", addMemberDto.Username, addMemberDto.ChatId);
        return Ok("User added to chat successfully.");
    }
}
