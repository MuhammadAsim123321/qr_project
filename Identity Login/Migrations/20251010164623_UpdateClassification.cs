using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Identity_Login.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClassification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ASFs",
                keyColumn: "ASFId",
                keyValue: 1,
                column: "Name",
                value: "8 ASF");

            migrationBuilder.UpdateData(
                table: "ASFs",
                keyColumn: "ASFId",
                keyValue: 2,
                column: "Name",
                value: "10 ASF");

            migrationBuilder.UpdateData(
                table: "ASFs",
                keyColumn: "ASFId",
                keyValue: 3,
                column: "Name",
                value: "12 ASF");

            migrationBuilder.UpdateData(
                table: "ASFs",
                keyColumn: "ASFId",
                keyValue: 4,
                column: "Name",
                value: "16 ASF");

            migrationBuilder.UpdateData(
                table: "ASFs",
                keyColumn: "ASFId",
                keyValue: 5,
                column: "Name",
                value: "24 ASF");

            migrationBuilder.InsertData(
                table: "classifications",
                columns: new[] { "ClassificationId", "Minutes", "Name" },
                values: new object[,]
                {
                    { 23, 30, "ASTM A 967, NITRIC" },
                    { 24, 30, "ASTM A 967, CITRIC" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 24);

            migrationBuilder.UpdateData(
                table: "ASFs",
                keyColumn: "ASFId",
                keyValue: 1,
                column: "Name",
                value: "8  ASF");

            migrationBuilder.UpdateData(
                table: "ASFs",
                keyColumn: "ASFId",
                keyValue: 2,
                column: "Name",
                value: "10  ASF");

            migrationBuilder.UpdateData(
                table: "ASFs",
                keyColumn: "ASFId",
                keyValue: 3,
                column: "Name",
                value: "12  ASF");

            migrationBuilder.UpdateData(
                table: "ASFs",
                keyColumn: "ASFId",
                keyValue: 4,
                column: "Name",
                value: "16  ASF");

            migrationBuilder.UpdateData(
                table: "ASFs",
                keyColumn: "ASFId",
                keyValue: 5,
                column: "Name",
                value: "24  ASF");
        }
    }
}
