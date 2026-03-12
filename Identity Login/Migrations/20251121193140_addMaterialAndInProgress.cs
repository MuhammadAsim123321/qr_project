using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Identity_Login.Migrations
{
    /// <inheritdoc />
    public partial class addMaterialAndInProgress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DeleteData(
            //    table: "classifications",
            //    keyColumn: "ClassificationId",
            //    keyValue: 26);

            //migrationBuilder.DeleteData(
            //    table: "classifications",
            //    keyColumn: "ClassificationId",
            //    keyValue: 27);

            migrationBuilder.InsertData(
                table: "Materails",
                columns: new[] { "MaterailId", "Name" },
                values: new object[,]
                {
                    { 5, "Stainless steel" },
                    { 6, "Steel" }
                });

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 4,
                column: "StepOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 5,
                column: "StepOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 6,
                column: "StepOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 7,
                column: "StepOrder",
                value: 8);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 8,
                column: "StepOrder",
                value: 9);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 12,
                column: "StepOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 13,
                column: "StepOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 14,
                column: "StepOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 15,
                column: "StepOrder",
                value: 8);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 16,
                column: "StepOrder",
                value: 9);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 20,
                column: "StepOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 21,
                column: "StepOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 22,
                column: "StepOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 23,
                column: "StepOrder",
                value: 8);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 24,
                column: "StepOrder",
                value: 9);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 27,
                column: "StepOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 28,
                column: "StepOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 29,
                column: "StepOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 30,
                column: "StepOrder",
                value: 7);

            migrationBuilder.InsertData(
                table: "ProcessSteps",
                columns: new[] { "ProcessStepId", "CreatedBy", "CreatedOn", "IsDeleted", "IsOptional", "ProcessId", "StepName", "StepOrder", "UpdatedBy", "UpdatedOn" },
                values: new object[,]
                {
                    { 33, null, null, false, false, 1, "In-Process", 4, null, null },
                    { 34, null, null, false, false, 2, "In-Process", 4, null, null },
                    { 35, null, null, false, false, 3, "In-Process", 4, null, null },
                    { 36, null, null, false, false, 4, "In-Process", 3, null, null },
                    { 37, null, null, false, false, 5, "In-Process", 3, null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Materails",
                keyColumn: "MaterailId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Materails",
                keyColumn: "MaterailId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 37);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 4,
                column: "StepOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 5,
                column: "StepOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 6,
                column: "StepOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 7,
                column: "StepOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 8,
                column: "StepOrder",
                value: 8);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 12,
                column: "StepOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 13,
                column: "StepOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 14,
                column: "StepOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 15,
                column: "StepOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 16,
                column: "StepOrder",
                value: 8);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 20,
                column: "StepOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 21,
                column: "StepOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 22,
                column: "StepOrder",
                value: 6);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 23,
                column: "StepOrder",
                value: 7);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 24,
                column: "StepOrder",
                value: 8);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 27,
                column: "StepOrder",
                value: 3);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 28,
                column: "StepOrder",
                value: 4);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 29,
                column: "StepOrder",
                value: 5);

            migrationBuilder.UpdateData(
                table: "ProcessSteps",
                keyColumn: "ProcessStepId",
                keyValue: 30,
                column: "StepOrder",
                value: 6);

            //migrationBuilder.InsertData(
            //    table: "classifications",
            //    columns: new[] { "ClassificationId", "Minutes", "Name" },
            //    values: new object[,]
            //    {
            //        { 26, 0, "Stainless Steel" },
            //        { 27, 0, "Steel" }
            //    });
        }
    }
}
