using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Identity_Login.Migrations
{
    /// <inheritdoc />
    public partial class _4GoldTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "classifications",
                columns: new[] { "ClassificationId", "Minutes", "Name" },
                values: new object[,]
                {
                    { 28, 0, "Type I Gold Class 1A" },
                    { 29, 0, "Type I Gold Class 3" },
                    { 30, 0, "Type II Clear Class 1A" },
                    { 31, 0, "Type II Clear Class 3" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 31);
        }
    }
}
