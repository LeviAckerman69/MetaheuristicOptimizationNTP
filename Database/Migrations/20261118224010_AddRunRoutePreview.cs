using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetaheuristicOptimizationNTP.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddRunRoutePreview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RoutePreview",
                table: "ExperimentRuns",
                type: "BLOB",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoutePreview",
                table: "ExperimentRuns");
        }
    }
}
