using Microsoft.EntityFrameworkCore;
using RunGroups.Data;
using RunGroups.Interfaces;
using RunGroups.Models;

namespace RunGroups.Service
{
    public class ClubService : IClubService
    {
        private readonly ApplicationDbContext _context;
        public ClubService(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Club club) { _context.Add(club); return Save(); }
        public bool Delete(Club club) { _context.Remove(club); return Save(); }
        public bool Save() { int saved = _context.SaveChanges(); return saved > 0 ? true : false; }
        public bool Update(Club club) { throw new NotImplementedException(); }

        public async Task<IEnumerable<Club>> GetAll() 
        {
            return await _context.Clubs.ToListAsync();

        
        }
        public async Task<Club> GetByIdAsync(int id)
        {
            return await _context.Clubs.Include(a => a.Address).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Club>> GetClubByCity(string city)
        {
            return await _context.Clubs.Where(c => c.Address.City.Contains(city)).ToListAsync();
        }


    }
}
