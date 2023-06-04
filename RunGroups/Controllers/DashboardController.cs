using Microsoft.AspNetCore.Mvc;
using RunGroups.Data;
using RunGroups.DTOs.DashboardDTOs;
using RunGroups.Interfaces;
using RunGroups.Models;

namespace RunGroups.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            List<Race> userRaces = await _dashboardService.GetAllUserRaces();
            List<Club> userClubs = await _dashboardService.GetAllUserClubs();
            var dashboardDto = new DashboardDto()
            {
                Races = userRaces,
                Clubs = userClubs
            };
            
            return View(dashboardDto);
        }
    }
}
