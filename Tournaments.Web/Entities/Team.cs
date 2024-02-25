using Microsoft.EntityFrameworkCore;

namespace Tournaments.Web.Entities
{
	
	public class Team
	{
		public int TeamId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string OfficialWebsiteUrl { get; set; }
		public byte[] Logo { get; set; }
		public DateTime FoundationDate { get; set; }
		public virtual ICollection<TeamTournament> Tournaments { get; set; } = new List<TeamTournament>();
    }
}
