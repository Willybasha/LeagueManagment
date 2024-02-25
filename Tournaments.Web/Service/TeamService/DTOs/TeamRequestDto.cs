using Tournaments.Web.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Tournaments.Web.Service.TeamService.DTOs
{
    public class TeamRequestDto
    {
        public int TeamId { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public string OfficialWebsiteUrl { get; set; }
        public IFormFile Logo { get; set; }
        public DateTime FoundationDate { get; set; }
        public int SelectedTournamentId { get; set; }
        public IList<int> SelectedTournaments {get; set; }
        public IEnumerable<SelectListItem>? Touraments { get; set; }
    }
}
