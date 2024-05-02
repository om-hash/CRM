using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pal.Data.Migrations
{
    /// <inheritdoc />
    public partial class xxxx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SysDecisions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysDecisions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysDecisionTranslates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DecisionId = table.Column<int>(type: "int", nullable: false),
                    DecisionName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysDecisionTranslates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysDecisionTranslates_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysDecisionTranslates_SysDecisions_DecisionId",
                        column: x => x.DecisionId,
                        principalTable: "SysDecisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SysDecisionTranslates_DecisionId",
                table: "SysDecisionTranslates",
                column: "DecisionId");

            migrationBuilder.CreateIndex(
                name: "IX_SysDecisionTranslates_LanguageId",
                table: "SysDecisionTranslates",
                column: "LanguageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SysDecisionTranslates");

            migrationBuilder.DropTable(
                name: "SysDecisions");
        }
    }
}
