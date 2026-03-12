using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity_Login.Migrations
{
    /// <inheritdoc />
    public partial class AddRouterJobFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Date",
                table: "RouterJobs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DrawingNo",
                table: "RouterJobs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Materail",
                table: "RouterJobs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PartName",
                table: "RouterJobs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "RouterJobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RCVDBy",
                table: "RouterJobs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippedBy",
                table: "RouterJobs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VerbalNo",
                table: "RouterJobs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "RouterJobs");

            migrationBuilder.DropColumn(
                name: "DrawingNo",
                table: "RouterJobs");

            migrationBuilder.DropColumn(
                name: "Materail",
                table: "RouterJobs");

            migrationBuilder.DropColumn(
                name: "PartName",
                table: "RouterJobs");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "RouterJobs");

            migrationBuilder.DropColumn(
                name: "RCVDBy",
                table: "RouterJobs");

            migrationBuilder.DropColumn(
                name: "ShippedBy",
                table: "RouterJobs");

            migrationBuilder.DropColumn(
                name: "VerbalNo",
                table: "RouterJobs");
        }
    }
}
