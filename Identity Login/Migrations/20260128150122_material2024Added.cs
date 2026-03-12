using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity_Login.Migrations
{
    /// <inheritdoc />
    public partial class material2024Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Materails",
                columns: new[] { "MaterailId", "Name" },
                values: new object[] { 7, "Material 2024" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Materails",
                keyColumn: "MaterailId",
                keyValue: 7);
        }
    }
}
