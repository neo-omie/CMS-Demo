using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class filePathAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "MasterApostilles",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "GetAllApprovalMatrixContractDTOs",
                columns: table => new
                {
                    MasterApprovalMatrixContractId = table.Column<int>(type: "int", nullable: false),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproverName1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproverName2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproverName3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalRecords = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "GetAllApprovalMatrixMOUByIdDtos",
                columns: table => new
                {
                    MasterApprovalMatrixMOUId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproverName1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproverId1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproverName2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproverId2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproverName3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproverId3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfDays = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "GetAllApprovalMatrixMOUDtos",
                columns: table => new
                {
                    MasterApprovalMatrixMOUId = table.Column<int>(type: "int", nullable: false),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproverName1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproverName2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproverName3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalRecords = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "GetApostillesDtos",
                columns: table => new
                {
                    ValueId = table.Column<int>(type: "int", nullable: false),
                    ApostilleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    TotalRecords = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "GetApprovalMatrixContractByIdDtos",
                columns: table => new
                {
                    MasterApprovalMatrixContractId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproverName1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproverId1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproverName2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproverId2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproverName3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproverId3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfDays = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "GetCompanyDtos",
                columns: table => new
                {
                    TotalRecords = table.Column<int>(type: "int", nullable: false),
                    ValueId = table.Column<int>(type: "int", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "GetContractByIdDtos",
                columns: table => new
                {
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    ContractName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContractWithCompanyId = table.Column<int>(type: "int", nullable: false),
                    ContractWithCompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContractTypeId = table.Column<int>(type: "int", nullable: false),
                    ContractTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApostilleTypeId = table.Column<int>(type: "int", nullable: false),
                    ApostilleTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActualDocRefNo = table.Column<int>(type: "int", nullable: false),
                    RetainerContract = table.Column<int>(type: "int", nullable: false),
                    TermsAndConditions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTill = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RenewalFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RenewalTill = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddendumDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmpCustodianId = table.Column<int>(type: "int", nullable: false),
                    EmpCustodianName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Approver1Status = table.Column<int>(type: "int", nullable: false),
                    Approver2Status = table.Column<int>(type: "int", nullable: false),
                    Approver3Status = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "GetContractsDtos",
                columns: table => new
                {
                    ContractID = table.Column<int>(type: "int", nullable: false),
                    ContractName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContractType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToBeRenewedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddendumDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ApprovalPendingFrom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RenewalContractPerson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RenewalDueIn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalRecords = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "GetDepartmentsDtos",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalRecords = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "GetEscalationMatrixContractDtos",
                columns: table => new
                {
                    MatrixContractId = table.Column<int>(type: "int", nullable: false),
                    Escalation1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Escalation2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Escalation3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EscalationId1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EscalationId2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EscalationId3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TriggerDaysEscalation1 = table.Column<int>(type: "int", nullable: false),
                    TriggerDaysEscalation2 = table.Column<int>(type: "int", nullable: false),
                    TriggerDaysEscalation3 = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "GetEscalationMatrixMouDtos",
                columns: table => new
                {
                    MatrixMouId = table.Column<int>(type: "int", nullable: false),
                    Escalation1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Escalation2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Escalation3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EscalationId1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EscalationId2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EscalationId3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TriggerDaysEscalation1 = table.Column<int>(type: "int", nullable: false),
                    TriggerDaysEscalation2 = table.Column<int>(type: "int", nullable: false),
                    TriggerDaysEscalation3 = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    totalRecords = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.UpdateData(
                table: "MasterEmployees",
                keyColumn: "ValueId",
                keyValue: 1,
                column: "Password",
                value: "AQAAAAIAAYagAAAAEJNJ1g/v7qwdY4qwpHitNpEhcmWjMYog0uANS8l91YazttgA+KnfIpeMA6LeI6YsZg==");

            migrationBuilder.UpdateData(
                table: "MasterEmployees",
                keyColumn: "ValueId",
                keyValue: 2,
                column: "Password",
                value: "AQAAAAIAAYagAAAAENoPxpSIHjaL3bmgCxvLO7yTh2aph01Iwtxa5k8Yf0GW87FyNVGkeB+PItcF3+jcfA==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GetAllApprovalMatrixContractDTOs");

            migrationBuilder.DropTable(
                name: "GetAllApprovalMatrixMOUByIdDtos");

            migrationBuilder.DropTable(
                name: "GetAllApprovalMatrixMOUDtos");

            migrationBuilder.DropTable(
                name: "GetApostillesDtos");

            migrationBuilder.DropTable(
                name: "GetApprovalMatrixContractByIdDtos");

            migrationBuilder.DropTable(
                name: "GetCompanyDtos");

            migrationBuilder.DropTable(
                name: "GetContractByIdDtos");

            migrationBuilder.DropTable(
                name: "GetContractsDtos");

            migrationBuilder.DropTable(
                name: "GetDepartmentsDtos");

            migrationBuilder.DropTable(
                name: "GetEscalationMatrixContractDtos");

            migrationBuilder.DropTable(
                name: "GetEscalationMatrixMouDtos");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "MasterApostilles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.UpdateData(
                table: "MasterEmployees",
                keyColumn: "ValueId",
                keyValue: 1,
                column: "Password",
                value: "AQAAAAIAAYagAAAAECpQWn4z6pK6NB2zDwhCwWqREi1z+16rirMIBDnqaa9uWwgrD1jlBjBzcsWB6WO8Bw==");

            migrationBuilder.UpdateData(
                table: "MasterEmployees",
                keyColumn: "ValueId",
                keyValue: 2,
                column: "Password",
                value: "AQAAAAIAAYagAAAAENaW+Whtl60ymxL1Y/LJA6gqShgmONNJLdNJE+lnGYcuEUE6nC7MkQrkpCiAATqzuA==");
        }
    }
}
