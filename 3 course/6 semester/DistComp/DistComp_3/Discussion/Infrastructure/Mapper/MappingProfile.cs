using AutoMapper;
using Discussion.DTO.Request;
using Discussion.DTO.Response;
using Discussion.Models;

namespace Discussion.Infrastructure.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Notice, NoticeResponseDTO>();
        CreateMap<NoticeRequestDTO, Notice>();
    }
}