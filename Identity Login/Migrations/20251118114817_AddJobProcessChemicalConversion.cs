using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Identity_Login.Migrations
{
    /// <inheritdoc />
    public partial class AddJobProcessChemicalConversion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "JobProcesses",
                columns: new[] { "ProcessId", "CreatedBy", "CreatedOn", "Description", "IsDeleted", "Name", "UpdatedBy", "UpdatedOn" },
                values: new object[] { 5, null, null, "Handles chemical conversion jobs", false, "Chemical Conversion", null, null });

            migrationBuilder.InsertData(
                table: "ProcessSteps",
                columns: new[] { "ProcessStepId", "CreatedBy", "CreatedOn", "IsDeleted", "IsOptional", "ProcessId", "StepName", "StepOrder", "UpdatedBy", "UpdatedOn" },
                values: new object[,]
                {
                    { 31, null, null, false, false, 5, "Class 1A Thick Coating", 1, null, null },
                    { 32, null, null, false, false, 5, "Class 3 Thin Coating", 2, null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "JobProcesses",
                keyColumn: "ProcessId",
                keyValue: 5);
        }
    }
}
