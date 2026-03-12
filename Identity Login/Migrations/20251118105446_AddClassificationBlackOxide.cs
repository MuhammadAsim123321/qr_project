using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity_Login.Migrations
{
    /// <inheritdoc />
    public partial class AddClassificationBlackOxide : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "classifications",
                columns: new[] { "ClassificationId", "Minutes", "Name" },
                values: new object[] { 25, 0, "Black Oxide" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 25);
        }
    }
}
