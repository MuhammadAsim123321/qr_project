using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Identity_Login.Migrations
{
    /// <inheritdoc />
    public partial class Classification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassificationId",
                table: "RouterJobs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "classifications",
                columns: table => new
                {
                    ClassificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classifications", x => x.ClassificationId);
                });

            migrationBuilder.InsertData(
                table: "classifications",
                columns: new[] { "ClassificationId", "Name" },
                values: new object[,]
                {
                    { 1, "Type I, Class 1 (CLEAR)" },
                    { 2, "Type II, Class 1 (CLEAR)" },
                    { 3, "Type II, Class 2 (BLACK)" },
                    { 4, "Type II, Class 2 (BLUE-A)" },
                    { 5, "Type II, Class 2 (BORDEAUX RED)" },
                    { 6, "Type II, Class 2 (BROWN-GL)" },
                    { 7, "Type II, Class 2 (CAMO BROWN)" },
                    { 8, "Type II, Class 2 (DARK BLUE)" },
                    { 9, "Type II, Class 2 (GOLD S)" },
                    { 10, "Type II, Class 2 (GREEN AEN)" },
                    { 11, "Type II, Class 2 (GREY)" },
                    { 12, "Type II, Class 2 (LANTZ MEDICAL BLUE)" },
                    { 13, "Type II, Class 2 (NEON PINK)" },
                    { 14, "Type II, Class 2 (OLIVE DRAB)" },
                    { 15, "Type II, Class 2 (ORANGE 2B)" },
                    { 16, "Type II, Class 2 (TEAL)" },
                    { 17, "Type II, Class 2 (VIOLET 3D)" },
                    { 18, "Type II, Class 2 (YELLOW 4A)" },
                    { 19, "Type III, Class 1 (CLEAR)" },
                    { 20, "Type III, Class 1 (CLEAR) W/ PTFE TEFLON" },
                    { 21, "Type III, Class 1 (CLEAR)(2 MIL)" },
                    { 22, "Type III, Class 2 (BLACK)" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RouterJobs_ClassificationId",
                table: "RouterJobs",
                column: "ClassificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_RouterJobs_classifications_ClassificationId",
                table: "RouterJobs",
                column: "ClassificationId",
                principalTable: "classifications",
                principalColumn: "ClassificationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RouterJobs_classifications_ClassificationId",
                table: "RouterJobs");

            migrationBuilder.DropTable(
                name: "classifications");

            migrationBuilder.DropIndex(
                name: "IX_RouterJobs_ClassificationId",
                table: "RouterJobs");

            migrationBuilder.DropColumn(
                name: "ClassificationId",
                table: "RouterJobs");
        }
    }
}
