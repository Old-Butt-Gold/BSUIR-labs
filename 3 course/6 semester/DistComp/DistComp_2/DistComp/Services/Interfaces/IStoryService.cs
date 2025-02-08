using DistComp.DTO.RequestDTO;
using DistComp.DTO.ResponseDTO;

namespace DistComp.Services.Interfaces;

public interface IStoryService
{
    Task<IEnumerable<StoryResponseDTO>> GetStoriesAsync();

    Task<StoryResponseDTO> GetStoryByIdAsync(long id);

    Task<StoryResponseDTO> CreateStoryAsync(StoryRequestDTO story);

    Task<StoryResponseDTO> UpdateStoryAsync(StoryRequestDTO story);

    Task DeleteStoryAsync(long id);
}