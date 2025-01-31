using DistComp_1.DTO.RequestDTO;
using DistComp_1.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DistComp_1.Controllers.V1;

[ApiController]
[Route("api/v1.0/[controller]")]
public class TagsController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagsController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTags()
    {
        var tags = await _tagService.GetTagsAsync();
        return Ok(tags);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTagById(long id)
    {
        var tag = await _tagService.GetTagByIdAsync(id);
        return Ok(tag);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTag([FromBody] TagRequestDTO tag)
    {
        var createdTag = await _tagService.CreateTagAsync(tag);
        return CreatedAtAction(nameof(CreateTag), new { id = createdTag.Id }, createdTag);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTag([FromBody] TagRequestDTO tag)
    {
        var updatedTag = await _tagService.UpdateTagAsync(tag);
        return Ok(updatedTag);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTag(long id)
    {
        await _tagService.DeleteTagAsync(id);
        return NoContent();
    }
}