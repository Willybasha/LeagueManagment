using Tournaments.Web.Entities;

namespace Tournaments.Web.Repository.TeamRepository
{
    public interface ITeamRepository
    {
        Task<IEnumerable<Team>> GetTeams();
        void CreateTeam(Team team);
        Task SaveAsync();
    }
}
