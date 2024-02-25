using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tournaments.Web.Data.Migrations
{
    public partial class ModifyTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Team_Tournament");

            migrationBuilder.CreateTable(
                name: "TeamTournament",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    _TournamentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamTournament", x => new { x.TeamId, x._TournamentId });
                    table.ForeignKey(
                        name: "FK_TeamTournament_teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamTournament_tournaments__TournamentId",
                        column: x => x._TournamentId,
                        principalTable: "tournaments",
                        principalColumn: "_TournamentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamTournament__TournamentId",
                table: "TeamTournament",
                column: "_TournamentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamTournament");

            migrationBuilder.CreateTable(
                name: "Team_Tournament",
                columns: table => new
                {
                    TeamsTeamId = table.Column<int>(type: "int", nullable: false),
                    Tournaments_TournamentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team_Tournament", x => new { x.TeamsTeamId, x.Tournaments_TournamentId });
                    table.ForeignKey(
                        name: "FK_Team_Tournament_teams_TeamsTeamId",
                        column: x => x.TeamsTeamId,
                        principalTable: "teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Team_Tournament_tournaments_Tournaments_TournamentId",
                        column: x => x.Tournaments_TournamentId,
                        principalTable: "tournaments",
                        principalColumn: "_TournamentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Team_Tournament_Tournaments_TournamentId",
                table: "Team_Tournament",
                column: "Tournaments_TournamentId");
        }
    }
}
