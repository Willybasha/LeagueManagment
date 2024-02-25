/*using Microsoft.EntityFrameworkCore;
using Tournaments.Web.Data;
using Tournaments.Web.Entities;

namespace Tournaments.Web.Repository.TournamentRepository
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly ApplicationDbContext _context;
        public TournamentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreateTournament(_Tournament tournament) => _context.Set<_Tournament>().Add(tournament);

        public async Task<_Tournament> GetTournamentAsync(int id) =>
            await _context.Set<_Tournament>().Where(x => x._TournamentId.Equals(id)).SingleOrDefaultAsync();
        

        public async Task<IEnumerable<_Tournament>> GetTournaments()
            => await _context.Set<_Tournament>().ToListAsync();

        
        public async Task SaveAsync() => await _context.SaveChangesAsync();



    }
}
*/