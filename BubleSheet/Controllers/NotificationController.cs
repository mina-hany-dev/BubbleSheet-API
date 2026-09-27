using bubblesheet.Infrastracture.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BubleSheet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController(INotificationService notificationService) : ControllerBase
    {
        private readonly INotificationService _notificationService = notificationService;
        [HttpGet("get-all-notifications")]
        [Authorize]
        public async Task<IActionResult> GetAllNotifications()
        {
            var notifications = await _notificationService.GetStudentNotificationsAsync();
            return Ok(notifications);
        }
        [HttpGet("get-all-unread-notifications")]
        [Authorize]
        public async Task<IActionResult> GetunreadNotifications()
        {
            var notifications = await _notificationService.GetUnreadNotificationsAsync();
            return Ok(notifications);
        }
        [HttpGet("get-count-unread-notifications")]
        [Authorize]
        public async Task<IActionResult> GetCountOfunreadNotifications()
        {
            var notifications = await _notificationService.GetUnreadCountAsync();
            return Ok(notifications);
        }
        [HttpPost("Mark-Read")]
        [Authorize]
        public async Task<IActionResult> MarkRead([FromBody] MarkNotificationsAsReadDto dto)
        {
            await _notificationService.MarkAsReadAsync(dto);
            return Ok("Done");
        }
    }
}
