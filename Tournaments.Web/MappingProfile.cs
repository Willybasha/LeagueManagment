using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tournaments.Web.Entities;
using Tournaments.Web.Service.TeamService.DTOs;
using Tournaments.Web.Services.TournamentService.DTOs;

namespace Tournaments.Web
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {

            CreateMap<TeamRequestDto, Team>().ForMember(dest=>dest.Tournaments,opt=>opt.Ignore()).ForMember(p => p.Logo, opt => opt.Ignore());
            CreateMap<Team, TeamRequestDto>().ForMember(p => p.Logo, opt => opt.Ignore());
            CreateMap<Team, TeamResponseDto>().ReverseMap();
            CreateMap<_Tournament,SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src._TournamentId))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));


            CreateMap<TournamentRequestDto, _Tournament>()
                .ForMember(dest => dest.Teams, opt => opt.Ignore())
                .ForMember(l => l.Logo, opt => opt.Ignore());
            CreateMap<_Tournament, TournamentRequestDto>().ForMember(l => l.Logo, opt => opt.Ignore());
            CreateMap<_Tournament, TournamentResponseDto>().ReverseMap();
            CreateMap<Team, SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.TeamId))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));

        }
    }
}
