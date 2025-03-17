using Microsoft.AspNetCore.Mvc;
using Publisher.DTO.RequestDTO;
using Publisher.Services.Interfaces;

namespace Publisher.Controllers.V1;

[ApiController]
[Route("api/v1.0/[controller]")]
public class StoriesController : ControllerBase
{
    private readonly IStoryService _storyService;

    public StoriesController(IStoryService storyService)
    {
        _storyService = storyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetStories()
    {
        var stories = await _storyService.GetStoriesAsync();
        return Ok(stories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStoryById(long id)
    {
        var story = await _storyService.GetStoryByIdAsync(id);
        return Ok(story);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStory([FromBody] StoryRequestDTO story)
    {
        var createdStory = await _storyService.CreateStoryAsync(story);
        return CreatedAtAction(nameof(CreateStory), new { id = createdStory.Id }, createdStory);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateStory([FromBody] StoryRequestDTO story)
    {
        var updatedStory = await _storyService.UpdateStoryAsync(story);
        return Ok(updatedStory);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStory(long id)
    {
        await _storyService.DeleteStoryAsync(id);
        return NoContent();
    }
}