namespace Tournaments.Web.Service.TeamService.DTOs
{
    public class TeamResponseDto
    {

        public int TeamId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OfficialWebsiteUrl { get; set; }
        public byte[] Logo { get; set; }
        public DateTime FoundationDate { get; set; }

        public IEnumerable<string> Tournaments { get; set; } = null!;
    }
}
