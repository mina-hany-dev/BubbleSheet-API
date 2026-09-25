using BubleSheet.Services.Interfaces;
using Domain.bublesheet.Entities.enums;
using Domain.bublesheet.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BubleSheet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DashboardController(IDashboardService dashboardService) : ControllerBase
    {
        private readonly IDashboardService _dashboardService = dashboardService;
        // Student Actions
        [Authorize]
        [HttpGet("get-percentage")]
        public async Task<IActionResult> GetPercentage()
        {
            var Data = await _dashboardService.GetStudentSolvedData();
            return Ok(Data.Item3);
        }
        [Authorize]
        [HttpGet("get-no-exams")]
        public async Task<IActionResult> GetNoExams()
        {
            var Data = await _dashboardService.GetStudentSolvedData();
            return Ok(Data.Item1);
        }
        [Authorize]
        [HttpGet("get-data")]
        public async Task<IActionResult> StudentData()
        {
            int studentId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var Data = await _dashboardService.GetMainDataforStudent(studentId);
            return Ok(Data);
        }
        [Authorize]
        [HttpGet("get-years")]
        public async Task<IActionResult> GetYears(Levels level)
        {
            var Years = await _dashboardService.GetAllYearsByLevel(level);
            return Ok(Years);
        }
        [Authorize]
        [HttpGet("get-table")]
        public async Task<IActionResult> GetTable(int AcYear , bool IsStudent = true)
        {
            var result = await _dashboardService.GetTableStudent(AcYear,IsStudent);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("get-subjects")]
        public async Task<IActionResult> GetSubjects(int YearId)
        {
            var subjects = await _dashboardService.GetAllSubjects(YearId);
            return Ok(subjects);
        }
        //Admin Actions
        [Authorize(Roles = "Admin")]
        [HttpGet("get-Admin-data")]
        public async Task<IActionResult> GetDataOfAdminDashboard()
        {
            var Ads = await _dashboardService.GetMainDataforAdmin();
            return Ok(Ads);
        }
    }
}
