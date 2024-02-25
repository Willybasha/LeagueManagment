namespace Tournaments.Web.Entities
{
    public class TeamTournament
    {
        public int TeamId { get; set; }
        public Team? Team { get; set; }

        public int _TournamentId { get; set; }
        public _Tournament? Tournament { get; set; }
    }
}
