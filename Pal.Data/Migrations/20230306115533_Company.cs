using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pal.Data.Migrations
{
    /// <inheritdoc />
    public partial class Company : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    CompanyCategoryId = table.Column<int>(type: "int", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    FullAddress = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    WhatsApp = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ManagedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ManagerName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    ManagerEmail = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: true),
                    ComapnyLogo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AvrageRating = table.Column<float>(type: "real", nullable: true),
                    MainImgUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ApproveState = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TaxNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TaxDocumentImage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ManagerSignatureImage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsTearmAndConditionsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    FacebookURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstagramURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwitterURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    linkedinURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_SysCities_CityId",
                        column: x => x.CityId,
                        principalTable: "SysCities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Companies_SysCompanyCategories_CompanyCategoryId",
                        column: x => x.CompanyCategoryId,
                        principalTable: "SysCompanyCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CompanyTranslate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompId = table.Column<int>(type: "int", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyTranslate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyTranslate_Companies_CompId",
                        column: x => x.CompId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CompanyTranslate_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CityId",
                table: "Companies",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CompanyCategoryId",
                table: "Companies",
                column: "CompanyCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyTranslate_CompId",
                table: "CompanyTranslate",
                column: "CompId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyTranslate_LanguageId",
                table: "CompanyTranslate",
                column: "LanguageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyTranslate");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
