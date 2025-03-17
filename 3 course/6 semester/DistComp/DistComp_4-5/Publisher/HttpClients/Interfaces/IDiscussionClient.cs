using Publisher.DTO.RequestDTO;
using Publisher.DTO.ResponseDTO;

namespace Publisher.HttpClients.Interfaces;

public interface IDiscussionClient
{
    Task<IEnumerable<NoticeResponseDTO>?> GetNoticesAsync();

    Task<NoticeResponseDTO?> GetNoticeByIdAsync(long id);

    Task<NoticeResponseDTO?> CreateNoticeAsync(NoticeRequestDTO post);

    Task<NoticeResponseDTO?> UpdateNoticeAsync(NoticeRequestDTO post);

    Task DeleteNoticeAsync(long id);
}