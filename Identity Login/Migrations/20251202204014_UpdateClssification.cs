using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Identity_Login.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClssification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "classifications",
                columns: new[] { "ClassificationId", "Minutes", "Name" },
                values: new object[,]
                {
                    //{ 26, 0, "Stainless Steel" },
                    //{ 27, 0, "Steel" },
                    { 32, 10, "Chemical Conversion Class 3" },
                    { 33, 15, "Chemical Conversion Class 1A" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DeleteData(
            //    table: "classifications",
            //    keyColumn: "ClassificationId",
            //    keyValue: 26);

            //migrationBuilder.DeleteData(
            //    table: "classifications",
            //    keyColumn: "ClassificationId",
            //    keyValue: 27);

            migrationBuilder.DeleteData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "classifications",
                keyColumn: "ClassificationId",
                keyValue: 33);
        }
    }
}
