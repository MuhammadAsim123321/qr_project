using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity_Login.Migrations
{
    /// <inheritdoc />
    public partial class AddProcessStepForChemical : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 31,
                column: "StepOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 32,
                column: "StepOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 37,
                column: "StepOrder",
                value: 4);

            migrationBuilder.InsertData(
                table: "ProcessSteps",
                columns: new[] { "ProcessStepId", "CreatedBy", "CreatedOn", "IsDeleted", "IsOptional", "ProcessId", "StepName", "StepOrder", "UpdatedBy", "UpdatedOn" },
                values: new object[] { 38, null, null, false, false, 5, "Create router/work order", 1, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 38);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 31,
                column: "StepOrder",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 32,
                column: "StepOrder",
                value: 2);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 37,
                column: "StepOrder",
                value: 3);
        }
    }
}
