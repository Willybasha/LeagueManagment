using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tournaments.Web.Entities;

namespace Tournaments.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Team> teams { get; set; }

        public DbSet<TeamTournament> TeamTournament { get; set; }
        public DbSet<_Tournament> tournaments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TeamTournament>().HasKey(e => new { e.TeamId, e._TournamentId });

            builder.Entity<TeamTournament>()
            .HasOne(tt => tt.Team)
            .WithMany(t => t.Tournaments)
            .HasForeignKey(tt => tt.TeamId);

            builder.Entity<TeamTournament>()
            .HasOne(tt => tt.Tournament)
            .WithMany(t => t.Teams)
            .HasForeignKey(tt => tt._TournamentId);

            base.OnModelCreating(builder);
        }
    }
}
