namespace ChatMicroservice.Models.DTO;

public class ChatCreateDto
{
    /// <summary>
    /// Название чата (опционально).
    /// </summary>
    public string? ChatName { get; set; }

    /// <summary>
    /// Указывает, является ли чат групповым.
    /// </summary>
    public bool IsGroup { get; set; }
}