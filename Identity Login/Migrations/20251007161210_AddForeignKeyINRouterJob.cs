using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity_Login.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeyINRouterJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProcessId",
                table: "RouterJobs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RouterJobs_ProcessId",
                table: "RouterJobs",
                column: "ProcessId");

            migrationBuilder.AddForeignKey(
                name: "FK_RouterJobs_JobProcesses_ProcessId",
                table: "RouterJobs",
                column: "ProcessId",
                principalTable: "JobProcesses",
                principalColumn: "ProcessId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RouterJobs_JobProcesses_ProcessId",
                table: "RouterJobs");

            migrationBuilder.DropIndex(
                name: "IX_RouterJobs_ProcessId",
                table: "RouterJobs");

            migrationBuilder.DropColumn(
                name: "ProcessId",
                table: "RouterJobs");
        }
    }
}
