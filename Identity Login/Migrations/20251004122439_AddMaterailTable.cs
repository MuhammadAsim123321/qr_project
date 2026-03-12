using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Identity_Login.Migrations
{
    /// <inheritdoc />
    public partial class AddMaterailTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Materail",
                table: "RouterJobs");

            migrationBuilder.AddColumn<int>(
                name: "MaterailId",
                table: "RouterJobs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Materails",
                columns: table => new
                {
                    MaterailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materails", x => x.MaterailId);
                });

            migrationBuilder.InsertData(
                table: "Materails",
                columns: new[] { "MaterailId", "Name" },
                values: new object[,]
                {
                    { 1, "5052 AL" },
                    { 2, "6061 AL" },
                    { 3, "7000 AL" },
                    { 4, "MIC-6 AL" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RouterJobs_MaterailId",
                table: "RouterJobs",
                column: "MaterailId");

            migrationBuilder.AddForeignKey(
                name: "FK_RouterJobs_Materails_MaterailId",
                table: "RouterJobs",
                column: "MaterailId",
                principalTable: "Materails",
                principalColumn: "MaterailId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RouterJobs_Materails_MaterailId",
                table: "RouterJobs");

            migrationBuilder.DropTable(
                name: "Materails");

            migrationBuilder.DropIndex(
                name: "IX_RouterJobs_MaterailId",
                table: "RouterJobs");

            migrationBuilder.DropColumn(
                name: "MaterailId",
                table: "RouterJobs");

            migrationBuilder.AddColumn<string>(
                name: "Materail",
                table: "RouterJobs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
