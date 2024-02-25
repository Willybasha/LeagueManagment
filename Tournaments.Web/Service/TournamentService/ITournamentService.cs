
using Tournaments.Web.Service.TournamentService.DTOs;
using Tournaments.Web.Services.TournamentService.DTOs;

namespace Tournaments.Web.Services.TournamentsService
{
    public interface ITournamentService
    {
        Task<IEnumerable<TournamentResponseDto>> GetListOfTournamnets();
        Task<TournamentResponseDto> CreateTournament(TournamentRequestDto tournament);
        Task<TournamentForUpdateDto> UpdateTournament(int tournamentId);

        Task DeleteTournament(int tournamentid);
    }
}
