using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Identity_Login.Migrations
{
    /// <inheritdoc />
    public partial class schemaClassesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.CreateTable(
                name: "JobProcesses",
                columns: table => new
                {
                    ProcessId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobProcesses", x => x.ProcessId);
                    table.ForeignKey(
                        name: "FK_JobProcesses_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JobProcesses_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RouterJobs",
                columns: table => new
                {
                    JobId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    QrCodeData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PdfFilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouterJobs", x => x.JobId);
                    table.ForeignKey(
                        name: "FK_RouterJobs_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RouterJobs_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProcessSteps",
                columns: table => new
                {
                    ProcessStepId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessId = table.Column<int>(type: "int", nullable: false),
                    StepName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StepOrder = table.Column<int>(type: "int", nullable: false),
                    IsOptional = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessSteps", x => x.ProcessStepId);
                    table.ForeignKey(
                        name: "FK_ProcessSteps_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProcessSteps_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProcessSteps_JobProcesses_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "JobProcesses",
                        principalColumn: "ProcessId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobProcessStages",
                columns: table => new
                {
                    JobProcessStageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    ProcessStepId = table.Column<int>(type: "int", nullable: false),
                    StageStatus = table.Column<int>(type: "int", nullable: false),
                    CompletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobProcessStages", x => x.JobProcessStageId);
                    table.ForeignKey(
                        name: "FK_JobProcessStages_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JobProcessStages_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JobProcessStages_ProcessSteps_ProcessStepId",
                        column: x => x.ProcessStepId,
                        principalTable: "ProcessSteps",
                        principalColumn: "ProcessStepId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobProcessStages_RouterJobs_JobId",
                        column: x => x.JobId,
                        principalTable: "RouterJobs",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stations",
                columns: table => new
                {
                    StationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProcessStepId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.StationId);
                    table.ForeignKey(
                        name: "FK_Stations_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Stations_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Stations_ProcessSteps_ProcessStepId",
                        column: x => x.ProcessStepId,
                        principalTable: "ProcessSteps",
                        principalColumn: "ProcessStepId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaffStationMappings",
                columns: table => new
                {
                    MappingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StationId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffStationMappings", x => x.MappingId);
                    table.ForeignKey(
                        name: "FK_StaffStationMappings_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StaffStationMappings_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StaffStationMappings_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StaffStationMappings_Stations_StationId",
                        column: x => x.StationId,
                        principalTable: "Stations",
                        principalColumn: "StationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "JobProcesses",
                columns: new[] { "ProcessId", "CreatedBy", "CreatedOn", "Description", "IsDeleted", "Name", "UpdatedBy", "UpdatedOn" },
                values: new object[,]
                {
                    { 1, null, null, "Handles anodizing jobs", false, "Anodizing Process", null, null },
                    { 2, null, null, "Handles passivation jobs", false, "Passivation Process", null, null },
                    { 3, null, null, "Handles black oxide jobs", false, "Black Oxide Process", null, null }
                });

            migrationBuilder.InsertData(
                table: "ProcessSteps",
                columns: new[] { "ProcessStepId", "CreatedBy", "CreatedOn", "IsDeleted", "IsOptional", "ProcessId", "StepName", "StepOrder", "UpdatedBy", "UpdatedOn" },
                values: new object[,]
                {
                    { 1, null, null, false, false, 1, "Create router/work order then parts are ready for racking", 1, null, null },
                    { 2, null, null, false, false, 1, "Ready for racking", 2, null, null },
                    { 3, null, null, false, false, 1, "Rack parts", 3, null, null },
                    { 4, null, null, false, false, 1, "Masking (some parts may need this process)", 4, null, null },
                    { 5, null, null, false, false, 1, "Anodize Process or Chemical conversion", 5, null, null },
                    { 6, null, null, false, false, 1, "Pack up parts", 6, null, null },
                    { 7, null, null, false, false, 1, "Ready for shipping", 7, null, null },
                    { 8, null, null, false, false, 2, "Create router/work order then parts are ready for racking", 1, null, null },
                    { 9, null, null, false, false, 2, "Ready for racking", 2, null, null },
                    { 10, null, null, false, false, 2, "Rack parts", 3, null, null },
                    { 11, null, null, false, false, 2, "Passivation process", 4, null, null },
                    { 12, null, null, false, false, 2, "Dry parts", 5, null, null },
                    { 13, null, null, false, false, 2, "Pack up parts", 6, null, null },
                    { 14, null, null, false, false, 2, "Ready for shipping", 7, null, null },
                    { 15, null, null, false, false, 2, "Shipped", 8, null, null },
                    { 16, null, null, false, false, 3, "Create router/work order then parts are ready for black oxide", 1, null, null },
                    { 17, null, null, false, false, 3, "Ready for Black Oxide", 2, null, null },
                    { 18, null, null, false, false, 3, "Black oxide parts", 3, null, null },
                    { 19, null, null, false, false, 3, "Pack up parts", 4, null, null },
                    { 20, null, null, false, false, 3, "Ready for Shipping", 5, null, null },
                    { 21, null, null, false, false, 3, "Shipped", 6, null, null },
                    { 22, null, null, false, false, 1, "Shipped", 8, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobProcesses_CreatedBy",
                table: "JobProcesses",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_JobProcesses_UpdatedBy",
                table: "JobProcesses",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_JobProcessStages_CreatedBy",
                table: "JobProcessStages",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_JobProcessStages_JobId",
                table: "JobProcessStages",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobProcessStages_ProcessStepId",
                table: "JobProcessStages",
                column: "ProcessStepId");

            migrationBuilder.CreateIndex(
                name: "IX_JobProcessStages_UpdatedBy",
                table: "JobProcessStages",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessSteps_CreatedBy",
                table: "ProcessSteps",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessSteps_ProcessId",
                table: "ProcessSteps",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessSteps_UpdatedBy",
                table: "ProcessSteps",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RouterJobs_CreatedBy",
                table: "RouterJobs",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RouterJobs_UpdatedBy",
                table: "RouterJobs",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StaffStationMappings_CreatedBy",
                table: "StaffStationMappings",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StaffStationMappings_Id",
                table: "StaffStationMappings",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_StaffStationMappings_StationId",
                table: "StaffStationMappings",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffStationMappings_UpdatedBy",
                table: "StaffStationMappings",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_CreatedBy",
                table: "Stations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_ProcessStepId",
                table: "Stations",
                column: "ProcessStepId");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_UpdatedBy",
                table: "Stations",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobProcessStages");

            migrationBuilder.DropTable(
                name: "StaffStationMappings");

            migrationBuilder.DropTable(
                name: "RouterJobs");

            migrationBuilder.DropTable(
                name: "Stations");

            migrationBuilder.DropTable(
                name: "ProcessSteps");

            migrationBuilder.DropTable(
                name: "JobProcesses");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
