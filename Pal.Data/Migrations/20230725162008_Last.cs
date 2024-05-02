using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pal.Data.Migrations
{
    /// <inheritdoc />
    public partial class Last : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_SysTaskStatus_StatusId",
                table: "Tasks");

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Tasks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ReferenceNumber",
                table: "Tasks",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReferenceType",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_SysTaskStatus_StatusId",
                table: "Tasks",
                column: "StatusId",
                principalTable: "SysTaskStatus",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_SysTaskStatus_StatusId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ReferenceType",
                table: "Tasks");

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReferenceNumber",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_SysTaskStatus_StatusId",
                table: "Tasks",
                column: "StatusId",
                principalTable: "SysTaskStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
