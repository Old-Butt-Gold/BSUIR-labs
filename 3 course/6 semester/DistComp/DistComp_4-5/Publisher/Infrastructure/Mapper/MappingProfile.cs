using AutoMapper;
using Publisher.DTO.RequestDTO;
using Publisher.DTO.ResponseDTO;
using Publisher.Models;

namespace Publisher.Infrastructure.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserResponseDTO>();
        CreateMap<UserRequestDTO, User>();

        CreateMap<Tag, TagResponseDTO>();
        CreateMap<TagRequestDTO, Tag>();

        CreateMap<Story, StoryResponseDTO>();
        CreateMap<StoryRequestDTO, Story>();
    }
}