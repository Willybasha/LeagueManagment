using Tournaments.Web.Data;
using Tournaments.Web.Entities;

using Microsoft.EntityFrameworkCore;

namespace Tournaments.Web.Repository.TeamRepository
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ApplicationDbContext _context;
        public TeamRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreateTeam(Team team) => _context.Set<Team>().Add(team);

        public async Task<IEnumerable<Team>> GetTeams()
            => await _context.Set<Team>().ToListAsync();

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}
