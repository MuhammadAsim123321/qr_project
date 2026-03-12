using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity_Login.Migrations
{
    /// <inheritdoc />
    public partial class RouterJobUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SurfaceArea",
                table: "RouterJobs",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SurfaceArea",
                table: "RouterJobs");
        }
    }
}
