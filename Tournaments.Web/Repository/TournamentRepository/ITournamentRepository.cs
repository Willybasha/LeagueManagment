using Tournaments.Web.Entities;

namespace Tournaments.Web.Repository.TournamentRepository
{
    public interface ITournamentRepository
    {
        Task<IEnumerable<_Tournament>> GetTournaments();
        void CreateTournament(_Tournament tournament);
        Task<_Tournament> GetTournamentAsync(int id);

        Task SaveAsync();
    }
}
