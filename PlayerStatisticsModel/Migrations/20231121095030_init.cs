using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlayerStatisticsModel.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "USER_STATISTICS",
                columns: table => new
                {
                    USERID = table.Column<int>(name: "USER_ID", type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DUELSWON = table.Column<int>(name: "DUELS_WON", type: "int", nullable: false),
                    DUELSLOST = table.Column<int>(name: "DUELS_LOST", type: "int", nullable: false),
                    DUELSDRAWN = table.Column<int>(name: "DUELS_DRAWN", type: "int", nullable: false),
                    DUELSPLAYED = table.Column<int>(name: "DUELS_PLAYED", type: "int", nullable: false),
                    AVGDUELDURATION = table.Column<float>(name: "AVG_DUEL_DURATION", type: "float", nullable: false),
                    LASTPLAYEDAT = table.Column<DateTime>(name: "LAST_PLAYED_AT", type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_STATISTICS", x => x.USERID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "USER_STATISTICS");
        }
    }
}
