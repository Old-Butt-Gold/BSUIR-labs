using System.IdentityModel.Tokens.Jwt;
using System.Text;
using MessageMicroservice.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MessageMicroservice.Models;
using MessageMicroservice.Models.DTO;

namespace MessageMicroservice.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class MessageController : ControllerBase
{
    readonly ApplicationContext _context;
    readonly IConfiguration _configuration;
    readonly UserIdProducer _userIdProducer;
    private readonly ChatIdProducer _chatIdProducer;
    private readonly ILogger<MessageController> _logger;

    public MessageController(ApplicationContext context, IConfiguration configuration, UserIdProducer userIdProducer, ChatIdProducer chatIdProducer, ILogger<MessageController> logger)
    {
        _context = context;
        _configuration = configuration;
        _userIdProducer = userIdProducer;
        _chatIdProducer = chatIdProducer;
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
            return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating token.");
            return null;
        }
    }
    
    /// <summary>
    /// Получить все сообщения по UserName.
    /// </summary>
    [HttpGet("messages-by-username")]
    public async Task<IActionResult> GetMessagesByUsername([FromQuery] string username)
    {
        LogRequest();
        _logger.LogInformation($"Fetching messages for user: {username}");

        var userId = await _userIdProducer.GetUserIdByUsernameAsync(username);
        if (userId == null)
        {
            _logger.LogWarning($"User not found: {username}");
            return NotFound("User not found.");
        }

        var messages = await _context.Messages
            .Where(m => m.SenderId == userId)
            .ToListAsync();

        _logger.LogInformation($"Retrieved {messages.Count} messages for user: {username}");
        return Ok(messages);
    }
    
    /// <summary>
    /// Получить все сообщения пользователя по UserName и ChatName.
    /// </summary>
    [HttpGet("messages-by-username-and-chatname")]
    public async Task<IActionResult> GetMessagesByUsernameAndChatName([FromQuery] string username, [FromQuery] string chatName)
    {
        LogRequest();
        _logger.LogInformation($"Fetching messages for user: {username} in chat: {chatName}");

        var userId = await _userIdProducer.GetUserIdByUsernameAsync(username);
        if (userId == null)
        {
            _logger.LogWarning($"User not found: {username}");
            return NotFound("User not found.");
        }

        var chatId = await _chatIdProducer.GetChatIdByChatNameAsync(chatName);
        if (chatId == null)
        {
            _logger.LogWarning($"Chat not found: {chatName}");
            return NotFound("Chat not found.");
        }

        var messages = await _context.Messages
            .Where(m => m.SenderId == userId && m.ChatId == chatId)
            .ToListAsync();

        _logger.LogInformation($"Retrieved {messages.Count} messages for user: {username} in chat: {chatName}");
        return Ok(messages);
    }

    /// <summary>
    /// Написать сообщение пользователя по UserName и ChatName.
    /// </summary>
    [HttpPost("send-message")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
    {
        LogRequest();
        _logger.LogInformation($"User {request.Username} is sending a message to chat: {request.ChatName}");

        if (string.IsNullOrEmpty(request.Content))
        {
            _logger.LogWarning("Message content is empty.");
            return BadRequest("Message content cannot be empty.");
        }

        var userId = await _userIdProducer.GetUserIdByUsernameAsync(request.Username);
        if (userId == null)
        {
            _logger.LogWarning($"User not found: {request.Username}");
            return NotFound("User not found.");
        }

        var chatId = await _chatIdProducer.GetChatIdByChatNameAsync(request.ChatName);
        if (chatId == null)
        {
            _logger.LogWarning($"Chat not found: {request.ChatName}");
            return NotFound("Chat not found.");
        }

        Message message = new Message
        {
            MessageId = Guid.NewGuid(),
            ChatId = chatId,
            SenderId = userId,
            Context = request.Content,
            CreatedAt = DateTime.UtcNow,
            IsEdited = false,
            IsDeleted = false
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation($"Message sent successfully by {request.Username} to chat: {request.ChatName}");
        return Ok("Message sent successfully.");
    }
}