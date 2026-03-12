using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity_Login.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProcessStep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 25,
                column: "StepName",
                value: "Create router/work order then parts are ready for racking");

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 26,
                column: "StepName",
                value: "Ready for racking");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 25,
                column: "StepName",
                value: "Create router/work order then parts are ready for black oxide");

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 26,
                column: "StepName",
                value: "Ready for Black Oxide");
        }
    }
}
