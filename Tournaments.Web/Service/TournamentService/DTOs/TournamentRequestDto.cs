using Tournaments.Web.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Tournaments.Web.Helpers;

namespace Tournaments.Web.Services.TournamentService.DTOs
{
    public class TournamentRequestDto
    {
        
        public int _TournamentId { get; set; }

        [MaxLength(100, ErrorMessage = Errors.MaxLength), Display(Name = "Tournament")]
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Logo { get; set; }
        public string TournamentVideoEmbed { get; set; }
        public int SelectedTeamId { get; set; }
        public IList<int> SelectedTeams { get; set; } = new List<int>();
        public IEnumerable<SelectListItem>? Teams { get; set; }

    }
}
