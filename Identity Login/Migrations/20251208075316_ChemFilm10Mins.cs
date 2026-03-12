using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity_Login.Migrations
{
    /// <inheritdoc />
    public partial class ChemFilm10Mins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 29,
                column: "Minutes",
                value: 10);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 31,
                column: "Minutes",
                value: 10);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 29,
                column: "Minutes",
                value: 30);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 31,
                column: "Minutes",
                value: 30);
        }
    }
}
