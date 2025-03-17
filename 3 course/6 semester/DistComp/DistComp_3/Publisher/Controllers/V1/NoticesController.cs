using Microsoft.AspNetCore.Mvc;
using Publisher.DTO.RequestDTO;
using Publisher.DTO.ResponseDTO;
using Publisher.HttpClients;
using Publisher.HttpClients.Interfaces;
using Publisher.Services.Interfaces;

namespace Publisher.Controllers.V1;

[ApiController]
[Route("api/v1.0/[controller]")]
public class NoticesController : ControllerBase
{
    private readonly IDiscussionClient _noticeClient;

    public NoticesController(IDiscussionClient noticeClient)
    {
        _noticeClient = noticeClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetNotices()
    {
        var notices = await _noticeClient.GetNoticesAsync();
        return Ok(notices);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNoticeById(long id)
    {
        var notice = await _noticeClient.GetNoticeByIdAsync(id);
        return Ok(notice);
    }

    [HttpPost]
    public async Task<IActionResult> CreateNotice([FromBody] NoticeRequestDTO notice)
    {
        var createdNotice = await _noticeClient.CreateNoticeAsync(notice);
        return CreatedAtAction(nameof(CreateNotice), new { id = createdNotice.Id }, createdNotice);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateNotice([FromBody] NoticeRequestDTO notice)
    {
        var updatedNotice = await _noticeClient.UpdateNoticeAsync(notice);
        return Ok(updatedNotice);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNotice(long id)
    {
        await _noticeClient.DeleteNoticeAsync(id);
        return NoContent();
    }
}