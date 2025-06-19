using Microsoft.AspNetCore.Mvc;
using CustomerSuccessCRM.Lib.Services;
using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Web.Models;

namespace CustomerSuccessCRM.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ICrmService _crmService;

        public DashboardController(ICrmService crmService)
        {
            _crmService = crmService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var dashboard = await _crmService.GetDashboardDataAsync();
                return View(dashboard);
            }
            catch (Exception ex)
            {
                // Log the exception
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardData()
        {
            try
            {
                var dashboard = await _crmService.GetDashboardDataAsync();
                return Json(dashboard);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
} 