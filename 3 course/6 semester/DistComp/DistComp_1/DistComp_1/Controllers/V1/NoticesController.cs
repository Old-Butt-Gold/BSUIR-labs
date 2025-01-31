using DistComp_1.DTO.RequestDTO;
using DistComp_1.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DistComp_1.Controllers.V1;

[ApiController]
[Route("api/v1.0/[controller]")]
public class NoticesController : ControllerBase
{
    private readonly INoticeService _noticeService;

    public NoticesController(INoticeService noticeService)
    {
        _noticeService = noticeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetNotices()
    {
        var notices = await _noticeService.GetNoticesAsync();
        return Ok(notices);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNoticeById(long id)
    {
        var notice = await _noticeService.GetNoticeByIdAsync(id);
        return Ok(notice);
    }

    [HttpPost]
    public async Task<IActionResult> CreateNotice([FromBody] NoticeRequestDTO notice)
    {
        var createdNotice = await _noticeService.CreateNoticeAsync(notice);
        return CreatedAtAction(nameof(CreateNotice), new { id = createdNotice.Id }, createdNotice);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateNotice([FromBody] NoticeRequestDTO notice)
    {
        var updatedNotice = await _noticeService.UpdateNoticeAsync(notice);
        return Ok(updatedNotice);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNotice(long id)
    {
        await _noticeService.DeleteNoticeAsync(id);
        return NoContent();
    }
}