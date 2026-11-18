using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetaheuristicOptimizationNTP.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddEvolutionSteps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TownSnapshot",
                table: "ExperimentRuns",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "EvolutionSteps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ExperimentRunId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StepNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    ParentAId = table.Column<int>(type: "INTEGER", nullable: false),
                    ParentAPermutation = table.Column<string>(type: "TEXT", nullable: false),
                    ParentAFitness = table.Column<double>(type: "REAL", nullable: false),
                    ParentBId = table.Column<int>(type: "INTEGER", nullable: false),
                    ParentBPermutation = table.Column<string>(type: "TEXT", nullable: false),
                    ParentBFitness = table.Column<double>(type: "REAL", nullable: false),
                    CrossoverAlgorithm = table.Column<string>(type: "TEXT", nullable: false),
                    CrossoverPermutation = table.Column<string>(type: "TEXT", nullable: false),
                    CrossoverFitness = table.Column<double>(type: "REAL", nullable: false),
                    MutationAlgorithm = table.Column<string>(type: "TEXT", nullable: false),
                    MutationPermutation = table.Column<string>(type: "TEXT", nullable: false),
                    MutationFitness = table.Column<double>(type: "REAL", nullable: false),
                    BestFitnessBefore = table.Column<double>(type: "REAL", nullable: false),
                    BestFitnessAfter = table.Column<double>(type: "REAL", nullable: false),
                    OffspringSurvived = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvolutionSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvolutionSteps_ExperimentRuns_ExperimentRunId",
                        column: x => x.ExperimentRunId,
                        principalTable: "ExperimentRuns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EvolutionSteps_ExperimentRunId",
                table: "EvolutionSteps",
                column: "ExperimentRunId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EvolutionSteps");

            migrationBuilder.DropColumn(
                name: "TownSnapshot",
                table: "ExperimentRuns");
        }
    }
}
