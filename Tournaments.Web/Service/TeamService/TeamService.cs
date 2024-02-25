/*using AutoMapper;
using Tournaments.Web.Entities;
using Tournaments.Web.Repository.TeamRepository;
using Tournaments.Web.Service.TeamService.DTOs;

namespace Tournaments.Web.Service.TeamService
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _repository;
        private readonly IMapper _mapper;
        public TeamService(ITeamRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TeamResponseDto> CreateTeam(TeamRequestDto team)
        {
            var teamentity = _mapper.Map<Team>(team);
            // here we should to convert the IFormFile to Byte[].

            byte[] bytes = null;

            using (var memoryStream = new MemoryStream())
            {
                team.Logo.CopyTo(memoryStream);
                bytes = memoryStream.ToArray(); // Assuming LogoData is a byte[] property in your Team model
            }
            teamentity.Logo = bytes;

            _repository.CreateTeam(teamentity);
            
            await _repository.SaveAsync();
            var teamtoreturn=_mapper.Map<TeamResponseDto>(teamentity);
            return teamtoreturn;
        }


        public async Task<IEnumerable<TeamResponseDto>> GetListOfTeams()
        {
            var teams = await _repository.GetTeams();

            var responsedto = _mapper.Map<IEnumerable<TeamResponseDto>>(teams); 

            return responsedto;
        }

    }
}
*/