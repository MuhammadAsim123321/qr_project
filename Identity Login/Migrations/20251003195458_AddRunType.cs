using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Identity_Login.Migrations
{
    /// <inheritdoc />
    public partial class AddRunType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RunTypeId",
                table: "RouterJobs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RunTypes",
                columns: table => new
                {
                    RunTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RunTypes", x => x.RunTypeId);
                });

            migrationBuilder.InsertData(
                table: "RunTypes",
                columns: new[] { "RunTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "ETCH 3 MIN" },
                    { 2, "ETCH 5 MIN" },
                    { 3, "EXPED" },
                    { 4, "NO ETCH" },
                    { 5, "NO ETCH - CAST MTRL" },
                    { 6, "NO ETCH/5 MIN DESMUT" },
                    { 7, "NO SEAL" },
                    { 8, "NO SEAL/PTFE TEFLON" },
                    { 9, "PTFE TEFLON" },
                    { 10, "RUN ON LG AL RACK" },
                    { 11, "RUN ON MD AL RACK" },
                    { 12, "RUN ON ROUND RACK" },
                    { 13, "RUN ON SQ RACK" },
                    { 14, "STRIP 10 MIN/ETCH 5 MIN" },
                    { 15, "STRP 6 MIN/ETCH 2 MIN" },
                    { 16, "STRIP ONLY" },
                    { 17, "STRIP/ RE-ANODIZE" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RouterJobs_RunTypeId",
                table: "RouterJobs",
                column: "RunTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RouterJobs_RunTypes_RunTypeId",
                table: "RouterJobs",
                column: "RunTypeId",
                principalTable: "RunTypes",
                principalColumn: "RunTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RouterJobs_RunTypes_RunTypeId",
                table: "RouterJobs");

            migrationBuilder.DropTable(
                name: "RunTypes");

            migrationBuilder.DropIndex(
                name: "IX_RouterJobs_RunTypeId",
                table: "RouterJobs");

            migrationBuilder.DropColumn(
                name: "RunTypeId",
                table: "RouterJobs");
        }
    }
}
