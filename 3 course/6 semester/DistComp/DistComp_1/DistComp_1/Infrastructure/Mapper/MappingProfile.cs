using AutoMapper;
using DistComp_1.DTO.RequestDTO;
using DistComp_1.DTO.ResponseDTO;
using DistComp_1.Models;

namespace DistComp_1.Infrastructure.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserResponseDTO>();
        CreateMap<UserRequestDTO, User>();

        CreateMap<Notice, NoticeResponseDTO>();
        CreateMap<NoticeRequestDTO, Notice>();

        CreateMap<Tag, TagResponseDTO>();
        CreateMap<TagRequestDTO, Tag>();

        CreateMap<Story, StoryResponseDTO>();
        CreateMap<StoryRequestDTO, Story>();
    }
}