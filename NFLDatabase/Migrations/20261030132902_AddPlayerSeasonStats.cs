using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NFLDatabase.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayerSeasonStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Teams_TeamId",
                table: "Players");

            migrationBuilder.CreateTable(
                name: "PlayerSeasonStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Season = table.Column<int>(type: "INTEGER", nullable: false),
                    GamesPlayed = table.Column<int>(type: "INTEGER", nullable: false),
                    PassingYards = table.Column<int>(type: "INTEGER", nullable: false),
                    PassingTouchdowns = table.Column<int>(type: "INTEGER", nullable: false),
                    InterceptionsThrown = table.Column<int>(type: "INTEGER", nullable: false),
                    RushingAttempts = table.Column<int>(type: "INTEGER", nullable: false),
                    RushingYards = table.Column<int>(type: "INTEGER", nullable: false),
                    RushingTouchdowns = table.Column<int>(type: "INTEGER", nullable: false),
                    Receptions = table.Column<int>(type: "INTEGER", nullable: false),
                    ReceivingYards = table.Column<int>(type: "INTEGER", nullable: false),
                    ReceivingTouchdowns = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalTackles = table.Column<int>(type: "INTEGER", nullable: false),
                    TacklesForLoss = table.Column<int>(type: "INTEGER", nullable: false),
                    Sacks = table.Column<double>(type: "REAL", nullable: false),
                    Interceptions = table.Column<int>(type: "INTEGER", nullable: false),
                    PassesDefended = table.Column<int>(type: "INTEGER", nullable: false),
                    ForcedFumbles = table.Column<int>(type: "INTEGER", nullable: false),
                    FieldGoalsMade = table.Column<int>(type: "INTEGER", nullable: false),
                    FieldGoalAttempts = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerSeasonStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerSeasonStats_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerSeasonStats_PlayerId_Season",
                table: "PlayerSeasonStats",
                columns: new[] { "PlayerId", "Season" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Teams_TeamId",
                table: "Players",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Teams_TeamId",
                table: "Players");

            migrationBuilder.DropTable(
                name: "PlayerSeasonStats");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Teams_TeamId",
                table: "Players",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }
    }
}
