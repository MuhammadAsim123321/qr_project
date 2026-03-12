using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity_Login.Migrations
{
    /// <inheritdoc />
    public partial class updateProcessStepLabel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 1,
                column: "StepName",
                value: "Create router/work order");

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 9,
                column: "StepName",
                value: "Create router/work order");

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 17,
                column: "StepName",
                value: "Create router/work order");

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 25,
                column: "StepName",
                value: "Create router/work order");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 1,
                column: "StepName",
                value: "Create router/work order then parts are ready for racking");

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 9,
                column: "StepName",
                value: "Create router/work order then parts are ready for racking");

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 17,
                column: "StepName",
                value: "Create router/work order then parts are ready for racking");

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 25,
                column: "StepName",
                value: "Create router/work order then parts are ready for racking");
        }
    }
}
