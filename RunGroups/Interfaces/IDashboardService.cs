using RunGroups.Models;

namespace RunGroups.Interfaces
{
    public interface IDashboardService
    {
        Task<List<Race>> GetAllUserRaces();
        Task<List<Club>> GetAllUserClubs();
    }
}
