using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionIncidentes.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSolutionSteps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SolutionSteps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    KnowledgeEntryId = table.Column<Guid>(type: "uuid", nullable: false),
                    StepNumber = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolutionSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolutionSteps_KnowledgeEntries_KnowledgeEntryId",
                        column: x => x.KnowledgeEntryId,
                        principalTable: "KnowledgeEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SolutionSteps_KnowledgeEntryId_StepNumber",
                table: "SolutionSteps",
                columns: new[] { "KnowledgeEntryId", "StepNumber" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolutionSteps");
        }
    }
}
