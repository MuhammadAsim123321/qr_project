using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Identity_Login.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProcessAndSteps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "JobProcesses",
                keyColumn: "ProcessId",
                keyValue: 2,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Handles passivation jobs (method 1)", "Passivation Process (Method 1)" });

            migrationBuilder.UpdateData(
                table: "JobProcesses",
                keyColumn: "ProcessId",
                keyValue: 3,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Handles passivation jobs (method 2)", "Passivation Process (Method 2)" });

            migrationBuilder.InsertData(
                table: "JobProcesses",
                columns: new[] { "ProcessId", "CreatedBy", "CreatedOn", "Description", "IsDeleted", "Name", "UpdatedBy", "UpdatedOn" },
                values: new object[] { 4, null, null, "Handles black oxide jobs", false, "Black Oxide Process", null, null });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 8,
                columns: new[] { "ProcessId", "StepName", "StepOrder" },
                values: new object[] { 1, "Shipped", 8 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 9,
                columns: new[] { "StepName", "StepOrder" },
                values: new object[] { "Create router/work order then parts are ready for racking", 1 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 10,
                columns: new[] { "StepName", "StepOrder" },
                values: new object[] { "Ready for racking", 2 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 11,
                columns: new[] { "StepName", "StepOrder" },
                values: new object[] { "Rack parts", 3 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 12,
                columns: new[] { "StepName", "StepOrder" },
                values: new object[] { "Passivation process", 4 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 13,
                columns: new[] { "StepName", "StepOrder" },
                values: new object[] { "Dry parts", 5 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 14,
                columns: new[] { "StepName", "StepOrder" },
                values: new object[] { "Pack up parts", 6 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 15,
                columns: new[] { "StepName", "StepOrder" },
                values: new object[] { "Ready for shipping", 7 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 16,
                columns: new[] { "ProcessId", "StepName", "StepOrder" },
                values: new object[] { 2, "Shipped", 8 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 17,
                columns: new[] { "StepName", "StepOrder" },
                values: new object[] { "Create router/work order then parts are ready for racking", 1 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 18,
                columns: new[] { "StepName", "StepOrder" },
                values: new object[] { "Ready for racking", 2 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 19,
                columns: new[] { "StepName", "StepOrder" },
                values: new object[] { "Rack parts", 3 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 20,
                columns: new[] { "StepName", "StepOrder" },
                values: new object[] { "Passivation process", 4 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 21,
                columns: new[] { "StepName", "StepOrder" },
                values: new object[] { "Dry parts", 5 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 22,
                columns: new[] { "ProcessId", "StepName", "StepOrder" },
                values: new object[] { 3, "Pack up parts", 6 });

            migrationBuilder.InsertData(
                table: "ProcessSteps",
                columns: new[] { "ProcessStepId", "CreatedBy", "CreatedOn", "IsDeleted", "IsOptional", "ProcessId", "StepName", "StepOrder", "UpdatedBy", "UpdatedOn" },
                values: new object[,]
                {
                    { 23, null, null, false, false, 3, "Ready for shipping", 7, null, null },
                    { 24, null, null, false, false, 3, "Shipped", 8, null, null },
                    { 25, null, null, false, false, 4, "Create router/work order then parts are ready for black oxide", 1, null, null },
                    { 26, null, null, false, false, 4, "Ready for Black Oxide", 2, null, null },
                    { 27, null, null, false, false, 4, "Black oxide parts", 3, null, null },
                    { 28, null, null, false, false, 4, "Pack up parts", 4, null, null },
                    { 29, null, null, false, false, 4, "Ready for Shipping", 5, null, null },
                    { 30, null, null, false, false, 4, "Shipped", 6, null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "JobProcesses",
                keyColumn: "ProcessId",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "JobProcesses",
                keyColumn: "ProcessId",
                keyValue: 2,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Handles passivation jobs", "Passivation Process" });

            migrationBuilder.UpdateData(
                table: "JobProcesses",
                keyColumn: "ProcessId",
                keyValue: 3,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Handles black oxide jobs", "Black Oxide Process" });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 8,
                columns: new[] { "ProcessId", "StepName", "StepOrder" },
                values: new object[] { 2, "Create router/work order then parts are ready for racking", 1 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 9,
                columns: new[] { "StepName", "StepOrder" },
                values: new object[] { "Ready for racking", 2 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 10,
                columns: new[] { "StepName", "StepOrder" },
                values: new object[] { "Rack parts", 3 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 11,
                columns: new[] { "StepName", "StepOrder" },
                values: new object[] { "Passivation process", 4 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 12,
                columns: new[] { "StepName", "StepOrder" },
                values: new object[] { "Dry parts", 5 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 13,
                columns: new[] { "StepName", "StepOrder" },
                values: new object[] { "Pack up parts", 6 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 14,
                columns: new[] { "StepName", "StepOrder" },
                values: new object[] { "Ready for shipping", 7 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 15,
                columns: new[] { "StepName", "StepOrder" },
                values: new object[] { "Shipped", 8 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 16,
                columns: new[] { "ProcessId", "StepName", "StepOrder" },
                values: new object[] { 3, "Create router/work order then parts are ready for black oxide", 1 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 17,
                columns: new[] { "StepName", "StepOrder" },
                values: new object[] { "Ready for Black Oxide", 2 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 18,
                columns: new[] { "StepName", "StepOrder" },
                values: new object[] { "Black oxide parts", 3 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 19,
                columns: new[] { "StepName", "StepOrder" },
                values: new object[] { "Pack up parts", 4 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 20,
                columns: new[] { "StepName", "StepOrder" },
                values: new object[] { "Ready for Shipping", 5 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 21,
                columns: new[] { "StepName", "StepOrder" },
                values: new object[] { "Shipped", 6 });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 22,
                columns: new[] { "ProcessId", "StepName", "StepOrder" },
                values: new object[] { 1, "Shipped", 8 });
        }
    }
}
