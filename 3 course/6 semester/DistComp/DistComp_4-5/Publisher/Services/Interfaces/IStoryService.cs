using Publisher.DTO.RequestDTO;
using Publisher.DTO.ResponseDTO;

namespace Publisher.Services.Interfaces;

public interface IStoryService
{
    Task<IEnumerable<StoryResponseDTO>> GetStoriesAsync();

    Task<StoryResponseDTO> GetStoryByIdAsync(long id);

    Task<StoryResponseDTO> CreateStoryAsync(StoryRequestDTO story);

    Task<StoryResponseDTO> UpdateStoryAsync(StoryRequestDTO story);

    Task DeleteStoryAsync(long id);
}