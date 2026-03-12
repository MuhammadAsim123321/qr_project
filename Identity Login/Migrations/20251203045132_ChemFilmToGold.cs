using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity_Login.Migrations
{
    /// <inheritdoc />
    public partial class ChemFilmToGold : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 28,
                column: "Name",
                value: "Type I Gold Class 1A Chem Film");

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 29,
                column: "Name",
                value: "Type I Gold Class 3 Chem Film");

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 30,
                column: "Name",
                value: "Type II Clear Class 1A Chem Film");

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 31,
                column: "Name",
                value: "Type II Clear Class 3 Chem Film");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 28,
                column: "Name",
                value: "Type I Gold Class 1A");

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 29,
                column: "Name",
                value: "Type I Gold Class 3");

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 30,
                column: "Name",
                value: "Type II Clear Class 1A");

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 31,
                column: "Name",
                value: "Type II Clear Class 3");
        }
    }
}
