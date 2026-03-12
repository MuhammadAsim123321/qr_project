using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity_Login.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClassifyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Minutes",
                table: "classifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 1,
                column: "Minutes",
                value: 12);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 2,
                column: "Minutes",
                value: 18);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 3,
                column: "Minutes",
                value: 45);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 4,
                column: "Minutes",
                value: 38);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 5,
                column: "Minutes",
                value: 38);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 6,
                column: "Minutes",
                value: 30);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 7,
                column: "Minutes",
                value: 18);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 8,
                column: "Minutes",
                value: 45);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 9,
                column: "Minutes",
                value: 30);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 10,
                column: "Minutes",
                value: 45);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 11,
                column: "Minutes",
                value: 12);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 12,
                column: "Minutes",
                value: 25);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 13,
                column: "Minutes",
                value: 30);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 14,
                column: "Minutes",
                value: 35);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 15,
                column: "Minutes",
                value: 38);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 16,
                column: "Minutes",
                value: 38);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 17,
                column: "Minutes",
                value: 45);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 18,
                column: "Minutes",
                value: 15);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 19,
                column: "Minutes",
                value: 105);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 20,
                column: "Minutes",
                value: 105);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 21,
                column: "Minutes",
                value: 140);

            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 22,
                column: "Minutes",
                value: 105);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Minutes",
                table: "classifications");
        }
    }
}
