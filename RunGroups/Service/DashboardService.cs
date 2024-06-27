using RunGroups.Data;
using RunGroups.Interfaces;
using RunGroups.Models;
using System.Security.Claims;

namespace RunGroups.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<Race>> GetAllUserRaces()
        {
            string currentUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            IQueryable<Race> userRaces = _context.Races.Where(r => r.AppUser.Id == currentUser);
            return userRaces.ToList();  
        }
        public async Task<List<Club>> GetAllUserClubs()
        {
            string currentUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            IQueryable<Club> userClubs = _context.Clubs.Where(r => r.AppUser.Id == currentUser);
            return userClubs.ToList();
        }
    }
}
