using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MetaheuristicOptimizationNTP.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Earlier versions used EnsureCreated, so these tables may already contain data.
            migrationBuilder.Sql("""
                CREATE TABLE IF NOT EXISTS "Towns" (
                    "Id" TEXT NOT NULL CONSTRAINT "PK_Towns" PRIMARY KEY,
                    "Name" TEXT NOT NULL,
                    "X" REAL NOT NULL,
                    "Y" REAL NOT NULL
                );
                CREATE TABLE IF NOT EXISTS "Users" (
                    "Id" TEXT NOT NULL CONSTRAINT "PK_Users" PRIMARY KEY,
                    "Name" TEXT NOT NULL,
                    "PasswordHash" TEXT NOT NULL
                );
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Towns");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
