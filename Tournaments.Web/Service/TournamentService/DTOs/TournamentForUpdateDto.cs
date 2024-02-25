using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Tournaments.Web.Helpers;

namespace Tournaments.Web.Service.TournamentService.DTOs
{
    public class TournamentForUpdateDto
    {
        [MaxLength(100, ErrorMessage = Errors.MaxLength), Display(Name = "Tournament")]
        [Remote("AllowItem", null!, AdditionalFields = "_TournamentId", ErrorMessage = Errors.Duplicated)]
        public int _TournamentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Logo { get; set; }
        public string TournamentVideoEmbed { get; set; }
    }
}
