namespace Tournaments.Web.Services.TournamentService.DTOs
{
    public class TournamentResponseDto
    {
        public int _TournamentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Logo { get; set; }
        public string TournamentVideoEmbed { get; set; }

        public IEnumerable<string> Teams { get; set; } = null!;


    }
}
