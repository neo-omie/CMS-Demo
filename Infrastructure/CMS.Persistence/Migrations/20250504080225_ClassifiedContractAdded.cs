using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMS.Persistence.Migrations
{
    /// <inheritdoc />Q
    public partial class ClassifiedContractAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Approver1Email",
                table: "GetContractByIdDtos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Approver1EmployeeCode",
                table: "GetContractByIdDtos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Approver2Email",
                table: "GetContractByIdDtos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Approver2EmployeeCode",
                table: "GetContractByIdDtos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Approver3Email",
                table: "GetContractByIdDtos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Approver3EmployeeCode",
                table: "GetContractByIdDtos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ClassifiedContracts",
                columns: table => new
                {
                    ClassifiedContractId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassifiedContractName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    ContractWithCompanyId = table.Column<int>(type: "int", nullable: false),
                    ContractTypeId = table.Column<int>(type: "int", nullable: false),
                    ApostilleTypeId = table.Column<int>(type: "int", nullable: false),
                    ActualDocRefNo = table.Column<int>(type: "int", nullable: false),
                    RetainerContract = table.Column<int>(type: "int", nullable: false),
                    TermsAndConditions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTill = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RenewalFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RenewalTill = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddendumDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmpCustodianId = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Approver1Status = table.Column<int>(type: "int", nullable: false),
                    Approver2Status = table.Column<int>(type: "int", nullable: false),
                    Approver3Status = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassifiedContracts", x => x.ClassifiedContractId);
                    table.ForeignKey(
                        name: "FK_ClassifiedContracts_MasterApostilles_ApostilleTypeId",
                        column: x => x.ApostilleTypeId,
                        principalTable: "MasterApostilles",
                        principalColumn: "ValueId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassifiedContracts_MasterApprovalMatrixContracts_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "MasterApprovalMatrixContracts",
                        principalColumn: "MasterApprovalMatrixContractId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassifiedContracts_MasterCompanies_ContractWithCompanyId",
                        column: x => x.ContractWithCompanyId,
                        principalTable: "MasterCompanies",
                        principalColumn: "ValueId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassifiedContracts_MasterEmployees_EmpCustodianId",
                        column: x => x.EmpCustodianId,
                        principalTable: "MasterEmployees",
                        principalColumn: "ValueId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassifiedContracts_contracts_ContractTypeId",
                        column: x => x.ContractTypeId,
                        principalTable: "contracts",
                        principalColumn: "ValueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "MasterEmployees",
                keyColumn: "ValueId",
                keyValue: 1,
                columns: new[] { "Password", "Unit" },
                values: new object[] { "AQAAAAIAAYagAAAAEFLZ07muU5HPtjPDkGtvn00VD3YlhRVza1O5ACaaIezP3zK99/GDcCTtsx1BgWUvhA==", "Thane" });

            migrationBuilder.UpdateData(
                table: "MasterEmployees",
                keyColumn: "ValueId",
                keyValue: 2,
                columns: new[] { "Password", "Unit" },
                values: new object[] { "AQAAAAIAAYagAAAAENWh7ar92/3gn6YDWejRNGIZagbXVhA/TgWZmUuY01Sck9s90wKVNEdUv3RGSXMt9g==", "Thane" });

            migrationBuilder.CreateIndex(
                name: "IX_ClassifiedContracts_ApostilleTypeId",
                table: "ClassifiedContracts",
                column: "ApostilleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassifiedContracts_ContractTypeId",
                table: "ClassifiedContracts",
                column: "ContractTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassifiedContracts_ContractWithCompanyId",
                table: "ClassifiedContracts",
                column: "ContractWithCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassifiedContracts_DepartmentId",
                table: "ClassifiedContracts",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassifiedContracts_EmpCustodianId",
                table: "ClassifiedContracts",
                column: "EmpCustodianId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassifiedContracts");

            migrationBuilder.DropColumn(
                name: "Approver1Email",
                table: "GetContractByIdDtos");

            migrationBuilder.DropColumn(
                name: "Approver1EmployeeCode",
                table: "GetContractByIdDtos");

            migrationBuilder.DropColumn(
                name: "Approver2Email",
                table: "GetContractByIdDtos");

            migrationBuilder.DropColumn(
                name: "Approver2EmployeeCode",
                table: "GetContractByIdDtos");

            migrationBuilder.DropColumn(
                name: "Approver3Email",
                table: "GetContractByIdDtos");

            migrationBuilder.DropColumn(
                name: "Approver3EmployeeCode",
                table: "GetContractByIdDtos");

            migrationBuilder.UpdateData(
                table: "MasterEmployees",
                keyColumn: "ValueId",
                keyValue: 1,
                columns: new[] { "Password", "Unit" },
                values: new object[] { "AQAAAAIAAYagAAAAEJNJ1g/v7qwdY4qwpHitNpEhcmWjMYog0uANS8l91YazttgA+KnfIpeMA6LeI6YsZg==", "Dadar" });

            migrationBuilder.UpdateData(
                table: "MasterEmployees",
                keyColumn: "ValueId",
                keyValue: 2,
                columns: new[] { "Password", "Unit" },
                values: new object[] { "AQAAAAIAAYagAAAAENoPxpSIHjaL3bmgCxvLO7yTh2aph01Iwtxa5k8Yf0GW87FyNVGkeB+PItcF3+jcfA==", "Dadar" });
        }
    }
}
