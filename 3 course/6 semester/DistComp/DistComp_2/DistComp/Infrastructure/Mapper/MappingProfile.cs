using AutoMapper;
using DistComp.DTO.RequestDTO;
using DistComp.DTO.ResponseDTO;
using DistComp.Models;

namespace DistComp.Infrastructure.Mapper;

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