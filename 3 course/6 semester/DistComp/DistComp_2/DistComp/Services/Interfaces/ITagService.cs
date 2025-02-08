using DistComp.DTO.RequestDTO;
using DistComp.DTO.ResponseDTO;

namespace DistComp.Services.Interfaces;

public interface ITagService
{
    Task<IEnumerable<TagResponseDTO>> GetTagsAsync();

    Task<TagResponseDTO> GetTagByIdAsync(long id);

    Task<TagResponseDTO> CreateTagAsync(TagRequestDTO tag);

    Task<TagResponseDTO> UpdateTagAsync(TagRequestDTO tag);

    Task DeleteTagAsync(long id);
}