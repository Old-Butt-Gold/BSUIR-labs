using Discussion.DTO.Request;
using Discussion.DTO.Response;

namespace Discussion.Services.Interfaces;

public interface INoticeService
{
    Task<IEnumerable<NoticeResponseDTO>> GetNoticesAsync();

    Task<NoticeResponseDTO> GetNoticeByIdAsync(long id);

    Task<NoticeResponseDTO> CreateNoticeAsync(NoticeRequestDTO notice);

    Task<NoticeResponseDTO> UpdateNoticeAsync(NoticeRequestDTO notice);

    Task DeleteNoticeAsync(long id);
}