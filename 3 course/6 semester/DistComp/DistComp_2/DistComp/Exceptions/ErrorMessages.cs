namespace DistComp.Exceptions;

public static class ErrorMessages
{
    public static string UserNotFoundMessage(long id) => $"User with id {id} was not found.";
    public static string StoryNotFoundMessage(long id) => $"Story with id {id} was not found.";
    public static string TagNotFoundMessage(long id) => $"Tag with id {id} was not found.";
    public static string NoticeNotFoundMessage(long id) => $"Notice with id {id} was not found.";
}