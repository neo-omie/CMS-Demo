using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CMS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "contracts",
                columns: table => new
                {
                    ValueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contracts", x => x.ValueId);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Countries = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

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
                name: "MasterApostilles",
                columns: table => new
                {
                    ValueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApostilleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterApostilles", x => x.ValueId);
                });

            migrationBuilder.CreateTable(
                name: "MasterCompanies",
                columns: table => new
                {
                    ValueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PocName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyStatus = table.Column<bool>(type: "bit", nullable: false),
                    PocContactNumber = table.Column<int>(type: "int", nullable: false),
                    PocEmailId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyAddressLine1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyAddressLine2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyAddressLine3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Zipcode = table.Column<int>(type: "int", nullable: false),
                    CompanyContactNo = table.Column<int>(type: "int", nullable: false),
                    CompanyEmailId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyWebsiteUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyBankName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GSTno = table.Column<int>(type: "int", nullable: false),
                    BankAccNo = table.Column<int>(type: "int", nullable: false),
                    MSMERegistrationNo = table.Column<int>(type: "int", nullable: false),
                    IFSCCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PanNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    StateId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterCompanies", x => x.ValueId);
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
                name: "States",
                columns: table => new
                {
                    StateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.StateId);
                    table.ForeignKey(
                        name: "FK_States_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MasterEscalationMatrixContracts",
                columns: table => new
                {
                    MatrixContractId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Escalation1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Escalation2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Escalation3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TriggerDaysEscalation1 = table.Column<int>(type: "int", nullable: false),
                    TriggerDaysEscalation2 = table.Column<int>(type: "int", nullable: false),
                    TriggerDaysEscalation3 = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterEscalationMatrixContracts", x => x.MatrixContractId);
                    table.ForeignKey(
                        name: "FK_MasterEscalationMatrixContracts_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
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
                    ApproverId3 = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NumberOfDays = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterApprovalMatrixContracts", x => x.MasterApprovalMatrixContractId);
                    table.ForeignKey(
                        name: "FK_MasterApprovalMatrixContracts_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasterApprovalMatrixContracts_MasterEmployees_ApproverId1",
                        column: x => x.ApproverId1,
                        principalTable: "MasterEmployees",
                        principalColumn: "EmployeeCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasterApprovalMatrixContracts_MasterEmployees_ApproverId2",
                        column: x => x.ApproverId2,
                        principalTable: "MasterEmployees",
                        principalColumn: "EmployeeCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasterApprovalMatrixContracts_MasterEmployees_ApproverId3",
                        column: x => x.ApproverId3,
                        principalTable: "MasterEmployees",
                        principalColumn: "EmployeeCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MasterApprovalMatrixMOUs",
                columns: table => new
                {
                    MasterApprovalMatrixMOUId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    ApproverId1 = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApproverId2 = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApproverId3 = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NumberOfDays = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterApprovalMatrixMOUs", x => x.MasterApprovalMatrixMOUId);
                    table.ForeignKey(
                        name: "FK_MasterApprovalMatrixMOUs_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasterApprovalMatrixMOUs_MasterEmployees_ApproverId1",
                        column: x => x.ApproverId1,
                        principalTable: "MasterEmployees",
                        principalColumn: "EmployeeCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasterApprovalMatrixMOUs_MasterEmployees_ApproverId2",
                        column: x => x.ApproverId2,
                        principalTable: "MasterEmployees",
                        principalColumn: "EmployeeCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasterApprovalMatrixMOUs_MasterEmployees_ApproverId3",
                        column: x => x.ApproverId3,
                        principalTable: "MasterEmployees",
                        principalColumn: "EmployeeCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    CityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.CityId);
                    table.ForeignKey(
                        name: "FK_Cities_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "StateId",
                        onDelete: ReferentialAction.Cascade);
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
                table: "MasterDocuments",
                columns: new[] { "ValueId", "DocumentName", "IsDeleted", "status" },
                values: new object[,]
                {
                    { 1, "Doc 1", false, 1 },
                    { 2, "Doc 2", false, 1 },
                    { 3, "Doc 3", false, 1 }
                });

            migrationBuilder.InsertData(
                table: "MasterEmployees",
                columns: new[] { "ValueId", "DepartmentId", "Email", "EmployeeCode", "EmployeeExtension", "EmployeeMobile", "EmployeeName", "IsDeleted", "LastPasswordChanged", "Password", "Role", "Unit" },
                values: new object[,]
                {
                    { 1, 100, "admin@cms.com", "NEO1", "Main person", 7777766666L, "Admin", false, new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAIAAYagAAAAEDnk/CJ5AkYoOssgYoHGZ8FiKX9U5r8Blr3IYNZx+jTPnxyp8iezBQsUdXit1bEuYA==", "Admin", "Dadar" },
                    { 2, 101, "sarthak@neosoft.com", "NEO2", "IT Smart", 9999988888L, "Sarthak Lembhe", false, new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAIAAYagAAAAEAGbPOoZB6V1ozkhtTdePRzl3NHiSBHDSipX7jI5Vhp+/7HQTW1HcKxKawvSFOdG+w==", "MOU_User", "Dadar" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_StateId",
                table: "Cities",
                column: "StateId");

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

            migrationBuilder.CreateIndex(
                name: "IX_MasterApprovalMatrixMOUs_ApproverId1",
                table: "MasterApprovalMatrixMOUs",
                column: "ApproverId1");

            migrationBuilder.CreateIndex(
                name: "IX_MasterApprovalMatrixMOUs_ApproverId2",
                table: "MasterApprovalMatrixMOUs",
                column: "ApproverId2");

            migrationBuilder.CreateIndex(
                name: "IX_MasterApprovalMatrixMOUs_ApproverId3",
                table: "MasterApprovalMatrixMOUs",
                column: "ApproverId3");

            migrationBuilder.CreateIndex(
                name: "IX_MasterApprovalMatrixMOUs_DepartmentId",
                table: "MasterApprovalMatrixMOUs",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterEscalationMatrixContracts_DepartmentId",
                table: "MasterEscalationMatrixContracts",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_States_CountryId",
                table: "States",
                column: "CountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "contracts");

            migrationBuilder.DropTable(
                name: "MasterApostilles");

            migrationBuilder.DropTable(
                name: "MasterApprovalMatrixContracts");

            migrationBuilder.DropTable(
                name: "MasterApprovalMatrixMOUs");

            migrationBuilder.DropTable(
                name: "MasterCompanies");

            migrationBuilder.DropTable(
                name: "MasterDocuments");

            migrationBuilder.DropTable(
                name: "MasterEscalationMatrixContracts");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropTable(
                name: "MasterEmployees");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
