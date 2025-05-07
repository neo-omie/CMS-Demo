using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CMS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class sprint1Migration : Migration
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
                name: "GetClassifiedContractByIdDtos",
                columns: table => new
                {
                    ClassifiedContractId = table.Column<int>(type: "int", nullable: false),
                    ClassifiedContractName = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    Approver1Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Approver1EmployeeCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Approver2Status = table.Column<int>(type: "int", nullable: false),
                    Approver2Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Approver2EmployeeCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Approver3Status = table.Column<int>(type: "int", nullable: false),
                    Approver3Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Approver3EmployeeCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "GetClassifiedContractsDtos",
                columns: table => new
                {
                    ClassifiedContractID = table.Column<int>(type: "int", nullable: false),
                    ClassifiedContractName = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    Approver1Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Approver1EmployeeCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Approver2Status = table.Column<int>(type: "int", nullable: false),
                    Approver2Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Approver2EmployeeCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Approver3Status = table.Column<int>(type: "int", nullable: false),
                    Approver3Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Approver3EmployeeCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "MasterApostilles",
                columns: table => new
                {
                    ValueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApostilleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterApostilles", x => x.ValueId);
                });

            migrationBuilder.CreateTable(
                name: "MasterDocuments",
                columns: table => new
                {
                    ValueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayDocumentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UniqueDocumentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    EmployeeExtension = table.Column<int>(type: "int", nullable: false),
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
                        principalColumn: "CountryId");
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
                    table.UniqueConstraint("AK_MasterApprovalMatrixContracts_DepartmentId", x => x.DepartmentId);
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
                    table.UniqueConstraint("AK_MasterApprovalMatrixMOUs_DepartmentId", x => x.DepartmentId);
                    table.ForeignKey(
                        name: "FK_MasterApprovalMatrixMOUs_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId");
                    table.ForeignKey(
                        name: "FK_MasterApprovalMatrixMOUs_MasterEmployees_ApproverId1",
                        column: x => x.ApproverId1,
                        principalTable: "MasterEmployees",
                        principalColumn: "EmployeeCode");
                    table.ForeignKey(
                        name: "FK_MasterApprovalMatrixMOUs_MasterEmployees_ApproverId2",
                        column: x => x.ApproverId2,
                        principalTable: "MasterEmployees",
                        principalColumn: "EmployeeCode");
                    table.ForeignKey(
                        name: "FK_MasterApprovalMatrixMOUs_MasterEmployees_ApproverId3",
                        column: x => x.ApproverId3,
                        principalTable: "MasterEmployees",
                        principalColumn: "EmployeeCode");
                });

            migrationBuilder.CreateTable(
                name: "MasterEscalationMatrixContracts",
                columns: table => new
                {
                    MatrixContractId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EscalationId1 = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EscalationId2 = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EscalationId3 = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TriggerDaysEscalation1 = table.Column<int>(type: "int", nullable: false),
                    TriggerDaysEscalation2 = table.Column<int>(type: "int", nullable: false),
                    TriggerDaysEscalation3 = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterEscalationMatrixContracts", x => x.MatrixContractId);
                    table.UniqueConstraint("AK_MasterEscalationMatrixContracts_DepartmentId", x => x.DepartmentId);
                    table.ForeignKey(
                        name: "FK_MasterEscalationMatrixContracts_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId");
                    table.ForeignKey(
                        name: "FK_MasterEscalationMatrixContracts_MasterEmployees_EscalationId1",
                        column: x => x.EscalationId1,
                        principalTable: "MasterEmployees",
                        principalColumn: "EmployeeCode");
                    table.ForeignKey(
                        name: "FK_MasterEscalationMatrixContracts_MasterEmployees_EscalationId2",
                        column: x => x.EscalationId2,
                        principalTable: "MasterEmployees",
                        principalColumn: "EmployeeCode");
                    table.ForeignKey(
                        name: "FK_MasterEscalationMatrixContracts_MasterEmployees_EscalationId3",
                        column: x => x.EscalationId3,
                        principalTable: "MasterEmployees",
                        principalColumn: "EmployeeCode");
                });

            migrationBuilder.CreateTable(
                name: "MasterEscalationMatrixMous",
                columns: table => new
                {
                    MatrixMouId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EscalationId1 = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EscalationId2 = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EscalationId3 = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TriggerDaysEscalation1 = table.Column<int>(type: "int", nullable: false),
                    TriggerDaysEscalation2 = table.Column<int>(type: "int", nullable: false),
                    TriggerDaysEscalation3 = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterEscalationMatrixMous", x => x.MatrixMouId);
                    table.UniqueConstraint("AK_MasterEscalationMatrixMous_DepartmentId", x => x.DepartmentId);
                    table.ForeignKey(
                        name: "FK_MasterEscalationMatrixMous_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId");
                    table.ForeignKey(
                        name: "FK_MasterEscalationMatrixMous_MasterEmployees_EscalationId1",
                        column: x => x.EscalationId1,
                        principalTable: "MasterEmployees",
                        principalColumn: "EmployeeCode");
                    table.ForeignKey(
                        name: "FK_MasterEscalationMatrixMous_MasterEmployees_EscalationId2",
                        column: x => x.EscalationId2,
                        principalTable: "MasterEmployees",
                        principalColumn: "EmployeeCode");
                    table.ForeignKey(
                        name: "FK_MasterEscalationMatrixMous_MasterEmployees_EscalationId3",
                        column: x => x.EscalationId3,
                        principalTable: "MasterEmployees",
                        principalColumn: "EmployeeCode");
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
                        principalColumn: "StateId");
                });

            migrationBuilder.CreateTable(
                name: "MasterCompanies",
                columns: table => new
                {
                    ValueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PocName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyStatus = table.Column<bool>(type: "bit", nullable: false),
                    PocContactNumber = table.Column<long>(type: "bigint", nullable: false),
                    PocEmailId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyAddressLine1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyAddressLine2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyAddressLine3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Zipcode = table.Column<int>(type: "int", nullable: false),
                    CompanyContactNo = table.Column<long>(type: "bigint", nullable: false),
                    CompanyEmailId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyWebsiteUrl = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyBankName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GSTno = table.Column<long>(type: "bigint", nullable: false),
                    BankAccNo = table.Column<long>(type: "bigint", nullable: false),
                    MSMERegistrationNo = table.Column<long>(type: "bigint", nullable: false),
                    IFSCCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PanNo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    StateId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterCompanies", x => x.ValueId);
                    table.UniqueConstraint("AK_MasterCompanies_BankAccNo", x => x.BankAccNo);
                    table.UniqueConstraint("AK_MasterCompanies_CompanyContactNo", x => x.CompanyContactNo);
                    table.UniqueConstraint("AK_MasterCompanies_CompanyEmailId", x => x.CompanyEmailId);
                    table.UniqueConstraint("AK_MasterCompanies_CompanyName", x => x.CompanyName);
                    table.UniqueConstraint("AK_MasterCompanies_CompanyWebsiteUrl", x => x.CompanyWebsiteUrl);
                    table.UniqueConstraint("AK_MasterCompanies_GSTno", x => x.GSTno);
                    table.UniqueConstraint("AK_MasterCompanies_IFSCCode", x => x.IFSCCode);
                    table.UniqueConstraint("AK_MasterCompanies_MSMERegistrationNo", x => x.MSMERegistrationNo);
                    table.UniqueConstraint("AK_MasterCompanies_PanNo", x => x.PanNo);
                    table.UniqueConstraint("AK_MasterCompanies_PocEmailId", x => x.PocEmailId);
                    table.ForeignKey(
                        name: "FK_MasterCompanies_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "CityId");
                });

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
                        principalColumn: "ValueId");
                    table.ForeignKey(
                        name: "FK_ClassifiedContracts_MasterApprovalMatrixContracts_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "MasterApprovalMatrixContracts",
                        principalColumn: "MasterApprovalMatrixContractId");
                    table.ForeignKey(
                        name: "FK_ClassifiedContracts_MasterCompanies_ContractWithCompanyId",
                        column: x => x.ContractWithCompanyId,
                        principalTable: "MasterCompanies",
                        principalColumn: "ValueId");
                    table.ForeignKey(
                        name: "FK_ClassifiedContracts_MasterEmployees_EmpCustodianId",
                        column: x => x.EmpCustodianId,
                        principalTable: "MasterEmployees",
                        principalColumn: "ValueId");
                    table.ForeignKey(
                        name: "FK_ClassifiedContracts_contracts_ContractTypeId",
                        column: x => x.ContractTypeId,
                        principalTable: "contracts",
                        principalColumn: "ValueId");
                });

            migrationBuilder.CreateTable(
                name: "ContractsEntity",
                columns: table => new
                {
                    ContractId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractName = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_ContractsEntity", x => x.ContractId);
                    table.ForeignKey(
                        name: "FK_ContractsEntity_MasterApostilles_ApostilleTypeId",
                        column: x => x.ApostilleTypeId,
                        principalTable: "MasterApostilles",
                        principalColumn: "ValueId");
                    table.ForeignKey(
                        name: "FK_ContractsEntity_MasterApprovalMatrixContracts_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "MasterApprovalMatrixContracts",
                        principalColumn: "DepartmentId");
                    table.ForeignKey(
                        name: "FK_ContractsEntity_MasterCompanies_ContractWithCompanyId",
                        column: x => x.ContractWithCompanyId,
                        principalTable: "MasterCompanies",
                        principalColumn: "ValueId");
                    table.ForeignKey(
                        name: "FK_ContractsEntity_MasterEmployees_EmpCustodianId",
                        column: x => x.EmpCustodianId,
                        principalTable: "MasterEmployees",
                        principalColumn: "ValueId");
                    table.ForeignKey(
                        name: "FK_ContractsEntity_contracts_ContractTypeId",
                        column: x => x.ContractTypeId,
                        principalTable: "contracts",
                        principalColumn: "ValueId");
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
                    { 1, 1, "admin@cms.com", "NEO1", 2467, 7777766666L, "Admin", false, new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAIAAYagAAAAEIJdnQY1iMhSEQruYzBV5Qw8mezsE2lWyWNq6OY2NoePhFkKRNOmChEsE6NhpIqbiw==", "Admin", "Thane" },
                    { 2, 2, "sarthak@neosoft.com", "NEO2", 8976, 9999988888L, "Sarthak Lembhe", false, new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAIAAYagAAAAELMV5jrktbcQjWaNyvP06z+xH0Nf0PcMmU97aP7OoYimqoWZV4OvGaabFXLMmhzzHg==", "MOU_User", "Thane" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_StateId",
                table: "Cities",
                column: "StateId");

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

            migrationBuilder.CreateIndex(
                name: "IX_ContractsEntity_ApostilleTypeId",
                table: "ContractsEntity",
                column: "ApostilleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractsEntity_ContractTypeId",
                table: "ContractsEntity",
                column: "ContractTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractsEntity_ContractWithCompanyId",
                table: "ContractsEntity",
                column: "ContractWithCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractsEntity_DepartmentId",
                table: "ContractsEntity",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractsEntity_EmpCustodianId",
                table: "ContractsEntity",
                column: "EmpCustodianId");

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
                name: "IX_MasterCompanies_CityId",
                table: "MasterCompanies",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterEscalationMatrixContracts_EscalationId1",
                table: "MasterEscalationMatrixContracts",
                column: "EscalationId1");

            migrationBuilder.CreateIndex(
                name: "IX_MasterEscalationMatrixContracts_EscalationId2",
                table: "MasterEscalationMatrixContracts",
                column: "EscalationId2");

            migrationBuilder.CreateIndex(
                name: "IX_MasterEscalationMatrixContracts_EscalationId3",
                table: "MasterEscalationMatrixContracts",
                column: "EscalationId3");

            migrationBuilder.CreateIndex(
                name: "IX_MasterEscalationMatrixMous_EscalationId1",
                table: "MasterEscalationMatrixMous",
                column: "EscalationId1");

            migrationBuilder.CreateIndex(
                name: "IX_MasterEscalationMatrixMous_EscalationId2",
                table: "MasterEscalationMatrixMous",
                column: "EscalationId2");

            migrationBuilder.CreateIndex(
                name: "IX_MasterEscalationMatrixMous_EscalationId3",
                table: "MasterEscalationMatrixMous",
                column: "EscalationId3");

            migrationBuilder.CreateIndex(
                name: "IX_States_CountryId",
                table: "States",
                column: "CountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassifiedContracts");

            migrationBuilder.DropTable(
                name: "ContractsEntity");

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
                name: "GetClassifiedContractByIdDtos");

            migrationBuilder.DropTable(
                name: "GetClassifiedContractsDtos");

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

            migrationBuilder.DropTable(
                name: "MasterApprovalMatrixMOUs");

            migrationBuilder.DropTable(
                name: "MasterDocuments");

            migrationBuilder.DropTable(
                name: "MasterEscalationMatrixContracts");

            migrationBuilder.DropTable(
                name: "MasterEscalationMatrixMous");

            migrationBuilder.DropTable(
                name: "MasterApostilles");

            migrationBuilder.DropTable(
                name: "MasterApprovalMatrixContracts");

            migrationBuilder.DropTable(
                name: "MasterCompanies");

            migrationBuilder.DropTable(
                name: "contracts");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "MasterEmployees");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
