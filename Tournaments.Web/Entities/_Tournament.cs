namespace Tournaments.Web.Entities
{
    public class _Tournament
    {
        public int _TournamentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Logo { get; set; }
        public string TournamentVideoEmbed { get; set; }

        public virtual ICollection<TeamTournament> Teams { get; set; }= new List<TeamTournament>();
    }

}
