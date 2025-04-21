using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CMS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "MasterDocuments",
                columns: table => new
                {
                    ValueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterDocuments", x => x.ValueId);
                });

            migrationBuilder.CreateTable(
                name: "MasterEmployees",
                columns: table => new
                {
                    ValueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    EmployeeCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeMobile = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    LastPasswordChanged = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterEmployees", x => x.ValueId);
                    table.UniqueConstraint("AK_MasterEmployees_EmployeeCode", x => x.EmployeeCode);
                });

            migrationBuilder.CreateTable(
                name: "MasterApprovalMatrixContracts",
                columns: table => new
                {
                    MasterApprovalMatrixContractId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    ApproverId1 = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApproverId2 = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApproverId3 = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterApprovalMatrixContracts", x => x.MasterApprovalMatrixContractId);
                    table.ForeignKey(
                        name: "FK_MasterApprovalMatrixContracts_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId");
                    table.ForeignKey(
                        name: "FK_MasterApprovalMatrixContracts_MasterEmployees_ApproverId1",
                        column: x => x.ApproverId1,
                        principalTable: "MasterEmployees",
                        principalColumn: "EmployeeCode");
                    table.ForeignKey(
                        name: "FK_MasterApprovalMatrixContracts_MasterEmployees_ApproverId2",
                        column: x => x.ApproverId2,
                        principalTable: "MasterEmployees",
                        principalColumn: "EmployeeCode");
                    table.ForeignKey(
                        name: "FK_MasterApprovalMatrixContracts_MasterEmployees_ApproverId3",
                        column: x => x.ApproverId3,
                        principalTable: "MasterEmployees",
                        principalColumn: "EmployeeCode");
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "DepartmentName" },
                values: new object[,]
                {
                    { 1, "Admin Support" },
                    { 2, "IT" },
                    { 3, "HR" },
                    { 4, "Finance" },
                    { 5, "Maintenance" }
                });

            migrationBuilder.InsertData(
                table: "MasterEmployees",
                columns: new[] { "ValueId", "DepartmentId", "Email", "EmployeeCode", "EmployeeExtension", "EmployeeMobile", "EmployeeName", "IsDeleted", "LastPasswordChanged", "Password", "Role", "Unit" },
                values: new object[,]
                {
                    { 1, 100, "admin@cms.com", "NEO1", "Main person", 7777766666L, "Admin", false, new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAIAAYagAAAAEFmmYzRXEyTghAw2lAZyvoRdnBiTZTNNtsMLMACE0XCdS6dowDG+pSnhMNhEhVAucA==", "Admin", "Dadar" },
                    { 2, 101, "sarthak@neosoft.com", "NEO2", "IT Smart", 9999988888L, "Sarthak Lembhe", false, new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAIAAYagAAAAEN6pfxG4NPIHW2RyOt8EDhnzwuHuZB47x0+nSkLUEmVp0j2/ocPvIO9fcMa1+eFacQ==", "MOU_User", "Dadar" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MasterApprovalMatrixContracts_ApproverId1",
                table: "MasterApprovalMatrixContracts",
                column: "ApproverId1");

            migrationBuilder.CreateIndex(
                name: "IX_MasterApprovalMatrixContracts_ApproverId2",
                table: "MasterApprovalMatrixContracts",
                column: "ApproverId2");

            migrationBuilder.CreateIndex(
                name: "IX_MasterApprovalMatrixContracts_ApproverId3",
                table: "MasterApprovalMatrixContracts",
                column: "ApproverId3");

            migrationBuilder.CreateIndex(
                name: "IX_MasterApprovalMatrixContracts_DepartmentId",
                table: "MasterApprovalMatrixContracts",
                column: "DepartmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterApprovalMatrixContracts");

            migrationBuilder.DropTable(
                name: "MasterDocuments");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "MasterEmployees");
        }
    }
}
