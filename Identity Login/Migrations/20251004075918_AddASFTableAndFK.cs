using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Identity_Login.Migrations
{
    /// <inheritdoc />
    public partial class AddASFTableAndFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ASFId",
                table: "RouterJobs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ASFs",
                columns: table => new
                {
                    ASFId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ASFs", x => x.ASFId);
                });

            migrationBuilder.InsertData(
                table: "ASFs",
                columns: new[] { "ASFId", "Name" },
                values: new object[,]
                {
                    { 1, "8 ASF" },
                    { 2, "10 ASF" },
                    { 3, "12 ASF" },
                    { 4, "16 ASF" },
                    { 5, "24 ASF" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RouterJobs_ASFId",
                table: "RouterJobs",
                column: "ASFId");

            migrationBuilder.AddForeignKey(
                name: "FK_RouterJobs_ASFs_ASFId",
                table: "RouterJobs",
                column: "ASFId",
                principalTable: "ASFs",
                principalColumn: "ASFId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RouterJobs_ASFs_ASFId",
                table: "RouterJobs");

            migrationBuilder.DropTable(
                name: "ASFs");

            migrationBuilder.DropIndex(
                name: "IX_RouterJobs_ASFId",
                table: "RouterJobs");

            migrationBuilder.DropColumn(
                name: "ASFId",
                table: "RouterJobs");
        }
    }
}
