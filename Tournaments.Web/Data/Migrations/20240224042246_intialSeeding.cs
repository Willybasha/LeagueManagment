using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tournaments.Web.Data.Migrations
{
    public partial class intialSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "teams",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfficialWebsiteUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Logo = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    FoundationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teams", x => x.TeamId);
                });

            migrationBuilder.CreateTable(
                name: "tournaments",
                columns: table => new
                {
                    _TournamentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Logo = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    TournamentVideoEmbed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tournaments", x => x._TournamentId);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Team_Tournament");

            migrationBuilder.DropTable(
                name: "teams");

            migrationBuilder.DropTable(
                name: "tournaments");
        }
    }
}
