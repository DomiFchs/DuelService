using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlayerStatisticsModel.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AVG_DUEL_DURATION",
                table: "USER_STATISTICS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "AVG_DUEL_DURATION",
                table: "USER_STATISTICS",
                type: "float",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
