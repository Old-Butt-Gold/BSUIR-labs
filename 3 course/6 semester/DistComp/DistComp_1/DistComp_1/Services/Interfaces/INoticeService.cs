using DistComp_1.DTO.RequestDTO;
using DistComp_1.DTO.ResponseDTO;

namespace DistComp_1.Services.Interfaces;

public interface INoticeService
{
    Task<IEnumerable<NoticeResponseDTO>> GetNoticesAsync();

    Task<NoticeResponseDTO> GetNoticeByIdAsync(long id);

    Task<NoticeResponseDTO> CreateNoticeAsync(NoticeRequestDTO notice);

    Task<NoticeResponseDTO> UpdateNoticeAsync(NoticeRequestDTO notice);

    Task DeleteNoticeAsync(long id);
}