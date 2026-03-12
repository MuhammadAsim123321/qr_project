using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity_Login.Migrations
{
    /// <inheritdoc />
    public partial class ASF : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ASFs",
                keyColumn: "ASFId",
                keyValue: 6);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                table: "ASFs",
                columns: new[] { "ASFId", "Name" },
                values: new object[] { 6, "24 ASF" });
        }
    }
}
