namespace DistComp_1.Exceptions;

public static class ErrorMessages
{
    public static string UserNotFoundMessage(long id) => $"User with id {id} was not found.";
    public static string StoryNotFoundMessage(long id) => $"Story with id {id} was not found.";
    public static string TagNotFoundMessage(long id) => $"Tag with id {id} was not found.";
    public static string NoticeNotFoundMessage(long id) => $"Notice with id {id} was not found.";

    public static string UserAlreadyExists(string login) => $"User with login '{login}' already exists.";
    public static string StoryAlreadyExists(string title) => $"Story with title '{title}' already exists.";
    public static string TagAlreadyExists(string tag) => $"Tag with name '{tag}' already exists.";
}