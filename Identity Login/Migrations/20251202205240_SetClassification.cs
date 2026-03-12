using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Identity_Login.Migrations
{
    /// <inheritdoc />
    public partial class SetClassification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 33);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 28,
                column: "Minutes",
                value: 15);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 29,
                column: "Minutes",
                value: 30);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 30,
                column: "Minutes",
                value: 15);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 31,
                column: "Minutes",
                value: 30);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 28,
                column: "Minutes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 29,
                column: "Minutes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 30,
                column: "Minutes",
                value: 0);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 31,
                column: "Minutes",
                value: 0);

            migrationBuilder.InsertData(
                table: "classifications",
                columns: new[] { "ClassificationId", "Minutes", "Name" },
                values: new object[,]
                {
                    { 32, 10, "Chemical Conversion Class 3" },
                    { 33, 15, "Chemical Conversion Class 1A" }
                });
        }
    }
}
