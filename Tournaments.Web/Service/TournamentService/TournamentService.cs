using System;
using AutoMapper;
using Tournaments.Web.Entities;
using Tournaments.Web.Repository.TournamentRepository;
using Tournaments.Web.Service.TournamentService.DTOs;
using Tournaments.Web.Services.TournamentService.DTOs;
using Tournaments.Web.Services.TournamentsService;


namespace Tournament.Web.Service.TournamentService
{
    public class TournamentService : ITournamentService
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IMapper _mapper;
        public TournamentService(ITournamentRepository tournamentRepository,IMapper mapper) 
        {
            _mapper = mapper;
            _tournamentRepository = tournamentRepository;
        }

        public async Task<TournamentResponseDto> CreateTournament(TournamentRequestDto tournament)
        {
            var tournamententity = _mapper.Map<_Tournament>(tournament);
            byte[] bytes = null;

            using (var memoryStream = new MemoryStream())
            {
                tournament.Logo.CopyTo(memoryStream);
                bytes = memoryStream.ToArray(); // Assuming LogoData is a byte[] property in your Team model
            }
            tournamententity.Logo = bytes;
            _tournamentRepository.CreateTournament(tournamententity);

            await _tournamentRepository.SaveAsync();
            var tournamnettoreturn = _mapper.Map<TournamentResponseDto>(tournamententity);
            return tournamnettoreturn;
        }

        public Task DeleteTournament(int tournamentid)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TournamentResponseDto>> GetListOfTournamnets()
        {
            var tournaments= await _tournamentRepository.GetTournaments();

            var responsedto = _mapper.Map<IEnumerable<TournamentResponseDto>>(tournaments);

            return responsedto;

        }

        public async Task<TournamentForUpdateDto> UpdateTournament(int tournamentId)
        {
           var EntitytoUpdate=_tournamentRepository.GetTournamentAsync(tournamentId);

            if (EntitytoUpdate is null)
                throw new Exception("Not Found");

           var ViewModel= _mapper.Map<TournamentForUpdateDto>(EntitytoUpdate);

           await _tournamentRepository.SaveAsync();

            return ViewModel;
        }
    }
}
