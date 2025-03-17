using Publisher.DTO.RequestDTO;
using Publisher.DTO.ResponseDTO;

namespace Publisher.Services.Interfaces;

public interface ITagService
{
    Task<IEnumerable<TagResponseDTO>> GetTagsAsync();

    Task<TagResponseDTO> GetTagByIdAsync(long id);

    Task<TagResponseDTO> CreateTagAsync(TagRequestDTO tag);

    Task<TagResponseDTO> UpdateTagAsync(TagRequestDTO tag);

    Task DeleteTagAsync(long id);
}