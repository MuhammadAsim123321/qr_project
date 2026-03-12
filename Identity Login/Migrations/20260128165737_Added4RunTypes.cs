using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Identity_Login.Migrations
{
    /// <inheritdoc />
    public partial class Added4RunTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RunTypes",
                columns: new[] { "RunTypeId", "Name" },
                values: new object[,]
                {
                    { 18, "2 Min Etch" },
                    { 19, "Dichromate Seal" },
                    { 20, "Hot Water Seal" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RunTypes",
                keyColumn: "RunTypeId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "RunTypes",
                keyColumn: "RunTypeId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "RunTypes",
                keyColumn: "RunTypeId",
                keyValue: 20);
        }
    }
}
