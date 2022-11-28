using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StreamerApi.Migrations
{
    /// <inheritdoc />
    public partial class fixtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YouTubeLenght",
                table: "steamStats");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "YouTubeLenght",
                table: "steamStats",
                type: "text",
                nullable: true);
        }
    }
}
