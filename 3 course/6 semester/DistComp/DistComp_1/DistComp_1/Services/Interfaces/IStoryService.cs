using DistComp_1.DTO.RequestDTO;
using DistComp_1.DTO.ResponseDTO;

namespace DistComp_1.Services.Interfaces;

public interface IStoryService
{
    Task<IEnumerable<StoryResponseDTO>> GetStoriesAsync();

    Task<StoryResponseDTO> GetStoryByIdAsync(long id);

    Task<StoryResponseDTO> CreateStoryAsync(StoryRequestDTO story);

    Task<StoryResponseDTO> UpdateStoryAsync(StoryRequestDTO story);

    Task DeleteStoryAsync(long id);
}