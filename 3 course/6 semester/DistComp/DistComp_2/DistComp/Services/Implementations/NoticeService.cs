using AutoMapper;
using DistComp.DTO.RequestDTO;
using DistComp.DTO.ResponseDTO;
using DistComp.Exceptions;
using DistComp.Infrastructure.Validators;
using DistComp.Models;
using DistComp.Repositories.Interfaces;
using DistComp.Services.Interfaces;
using FluentValidation;

namespace DistComp.Services.Implementations;

public class NoticeService : INoticeService
{
    private readonly INoticeRepository _noticeRepository;
    private readonly IMapper _mapper;
    private readonly NoticeRequestDTOValidator _validator;
    
    public NoticeService(INoticeRepository noticeRepository, 
        IMapper mapper, NoticeRequestDTOValidator validator)
    {
        _noticeRepository = noticeRepository;
        _mapper = mapper;
        _validator = validator;
    }
    
    public async Task<IEnumerable<NoticeResponseDTO>> GetNoticesAsync()
    {
        var notices = await _noticeRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<NoticeResponseDTO>>(notices);
    }

    public async Task<NoticeResponseDTO> GetNoticeByIdAsync(long id)
    {
        var notice = await _noticeRepository.GetByIdAsync(id)
                      ?? throw new NotFoundException(ErrorCodes.NoticeNotFound, ErrorMessages.NoticeNotFoundMessage(id));
        return _mapper.Map<NoticeResponseDTO>(notice);
    }

    public async Task<NoticeResponseDTO> CreateNoticeAsync(NoticeRequestDTO notice)
    {
        await _validator.ValidateAndThrowAsync(notice);
        var noticeToCreate = _mapper.Map<Notice>(notice);
        var createdNotice = await _noticeRepository.CreateAsync(noticeToCreate);
        return _mapper.Map<NoticeResponseDTO>(createdNotice);
    }

    public async Task<NoticeResponseDTO> UpdateNoticeAsync(NoticeRequestDTO notice)
    {
        await _validator.ValidateAndThrowAsync(notice);
        var noticeToUpdate = _mapper.Map<Notice>(notice);
        var updatedNotice = await _noticeRepository.UpdateAsync(noticeToUpdate)
                             ?? throw new NotFoundException(ErrorCodes.NoticeNotFound, ErrorMessages.NoticeNotFoundMessage(notice.Id));
        return _mapper.Map<NoticeResponseDTO>(updatedNotice);
    }

    public async Task DeleteNoticeAsync(long id)
    {
        if (!await _noticeRepository.DeleteAsync(id))
        {
            throw new NotFoundException(ErrorCodes.NoticeNotFound, ErrorMessages.NoticeNotFoundMessage(id));
        }
    }
}
