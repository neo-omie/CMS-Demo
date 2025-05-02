CREATE OR ALTER PROCEDURE SP_GetAllContractsEntity @PageNumber int, @PageSize int
AS
DECLARE @TotalRecords int
BEGIN
	SELECT @TotalRecords = COUNT(ContractId) FROM ContractsEntity

	SELECT c.ContractId as ContractID, c.ContractName as ContractName,
	cc.ContractTypeName as ContractType, dd.DepartmentName as DepartmentName,
	c.ValidFrom as EffectiveDate, c.ValidTill as ExpiryDate,
	c.RenewalFrom as ToBeRenewedOn, c.RenewalTill as AddendumDate,
	c.Approver3Status as Status, me.EmployeeName as ApprovalPendingFrom,
	me.EmployeeName as RenewalContractPerson, CAST(CAST((CAST(c.RenewalTill as datetime) - GETDATE()) as int) as nvarchar(50)) as RenewalDueIn,
	c.Location as Location, @TotalRecords as TotalRecords
	FROM ContractsEntity c
	LEFT JOIN contracts cc ON cc.ValueId = c.ContractTypeId
	LEFT JOIN MasterApprovalMatrixContracts d ON d.DepartmentId = c.DepartmentId
	LEFT JOIN Departments dd ON dd.DepartmentId = c.DepartmentId
	LEFT JOIN MasterEmployees me ON me.EmployeeCode = d.ApproverId3
	WHERE c.IsDeleted = 0
	ORDER BY c.ContractId
	OFFSET(@PageNumber-1)*@PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
EXEC SP_GetAllContractsEntity @PageNumber = 1, @PageSize = 10;

SELECT CAST(CAST((CAST(c.RenewalTill as datetime) - GETDATE()) as int) as nvarchar(50)) as RenewalDueIn FROM ContractsEntity c;
SELECT CAST(RenewalTill as datetime) FROM ContractsEntity;

CREATE OR ALTER PROCEDURE SP_GetContractEntityByID @ID int
AS
BEGIN
	SELECT c.ContractId as ContractID, c.ContractName as ContractName,
	c.DepartmentId as DepartmentId, dd.DepartmentName as DepartmentName,
	c.ContractWithCompanyId as ContractWithCompanyId, mc.CompanyName as ContractWithCompanyName,
	c.ContractTypeId as ContractTypeId, cc.ContractTypeName as ContractTypeName,
	c.ApostilleTypeId as ApostilleTypeId, ma.ApostilleName as ApostilleTypeName,
	c.ActualDocRefNo as ActualDocRefNo, c.RetainerContract as RetainerContract,
	c.TermsAndConditions as TermsAndConditions,
	c.ValidFrom as ValidFrom, c.ValidTill as ValidTill,
	c.RenewalFrom as RenewalFrom, c.RenewalTill as RenewalTill,
	c.AddendumDate as AddendumDate,
	c.EmpCustodianId as EmpCustodianId, me.EmployeeName as EmpCustodianName,
	c.Location as Location, c.Approver1Status as Approver1Status,
	c.Approver2Status as Approver2Status, c.Approver3Status as Approver3Status,
	me1.EmployeeCode as Approver1EmployeeCode, me1.Email as Approver1Email,
	me2.EmployeeCode as Approver2EmployeeCode, me2.Email as Approver2Email,
	me3.EmployeeCode as Approver3EmployeeCode, me3.Email as Approver3Email,
	c.IsDeleted as IsDeleted
	FROM ContractsEntity c
	INNER JOIN contracts cc ON cc.ValueId = c.ContractTypeId
	INNER JOIN MasterApprovalMatrixContracts d ON d.DepartmentId = c.DepartmentId
	INNER JOIN Departments dd ON dd.DepartmentId = c.DepartmentId
	INNER JOIN MasterEmployees me ON me.ValueId = c.EmpCustodianId
	INNER JOIN MasterCompanies mc ON mc.ValueId = c.ContractWithCompanyId
	INNER JOIN MasterApostilles ma ON ma.ValueId = c.ApostilleTypeId
	INNER JOIN MasterEmployees me1 ON me1.EmployeeCode = d.ApproverId1
	INNER JOIN MasterEmployees me2 ON me2.EmployeeCode = d.ApproverId2
	INNER JOIN MasterEmployees me3 ON me3.EmployeeCode = d.ApproverId3
	WHERE c.ContractId = @ID;
END
EXEC SP_GetContractEntityByID @ID = 2;

SELECT CAST(CAST((CAST(c.RenewalTill as datetime) - GETDATE()) as int) as nvarchar(50)) as RenewalDueIn FROM ContractsEntity c;
SELECT CAST(RenewalTill as datetime) FROM ContractsEntity;