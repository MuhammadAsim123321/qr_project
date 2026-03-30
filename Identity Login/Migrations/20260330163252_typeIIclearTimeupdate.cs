using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity_Login.Migrations
{
    /// <inheritdoc />
    public partial class typeIIclearTimeupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 2,
                column: "Minutes",
                value: 27);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 2,
                column: "Minutes",
                value: 18);
        }
    }
}
