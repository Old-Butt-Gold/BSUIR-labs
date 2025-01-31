using DistComp_1.DTO.RequestDTO;
using DistComp_1.DTO.ResponseDTO;

namespace DistComp_1.Services.Interfaces;

public interface ITagService
{
    Task<IEnumerable<TagResponseDTO>> GetTagsAsync();

    Task<TagResponseDTO> GetTagByIdAsync(long id);

    Task<TagResponseDTO> CreateTagAsync(TagRequestDTO tag);

    Task<TagResponseDTO> UpdateTagAsync(TagRequestDTO tag);

    Task DeleteTagAsync(long id);
}