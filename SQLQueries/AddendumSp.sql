CREATE OR Alter PROCEDURE SP_GetAddendumContractByID @ID int
AS
BEGIN
	SELECT c.AddendumContractId as AddendumContractId, c.ContractId as ContractId, c.ContractName as ContractName,
	c.DepartmentId as DepartmentId, dd.DepartmentName as DepartmentName,
	c.ContractWithCompanyId as ContractWithCompanyId, mc.CompanyName as ContractWithCompanyName,
	c.ContractTypeId as ContractTypeId, cc.ContractTypeName as ContractTypeName,
	c.ApostilleTypeId as ApostilleTypeId, ma.ApostilleName as ApostilleTypeName,
	c.ActualDocRefNo as ActualDocRefNo, c.RetainerContract as RetainerContract,
	c.TermsAndConditions as TermsAndConditions,
	c.ValidFrom as ValidFrom, c.ValidTill as ValidTill,
	c.AddendumDate as AddendumDate,
	c.EmpCustodianId as EmpCustodianId, me.EmployeeName as EmpCustodianName, me.Email as EmpCustodianEmail,
	me.EmployeeCode as EmpCustodianCode,
	c.Location as Location, c.Approver1Status as Approver1Status,
	c.Approver2Status as Approver2Status, c.Approver3Status as Approver3Status,
	me1.EmployeeCode as Approver1EmployeeCode, me1.Email as Approver1Email,
	me2.EmployeeCode as Approver2EmployeeCode, me2.Email as Approver2Email,
	me3.EmployeeCode as Approver3EmployeeCode, me3.Email as Approver3Email,
	c.IsDeleted as IsDeleted
	FROM AddendumContracts c
	INNER JOIN contracts cc ON cc.ValueId = c.ContractTypeId
	INNER JOIN MasterApprovalMatrixContracts d ON d.DepartmentId = c.DepartmentId
	INNER JOIN Departments dd ON dd.DepartmentId = c.DepartmentId
	INNER JOIN MasterEmployees me ON me.ValueId = c.EmpCustodianId
	INNER JOIN MasterCompanies mc ON mc.ValueId = c.ContractWithCompanyId
	INNER JOIN MasterApostilles ma ON ma.ValueId = c.ApostilleTypeId
	INNER JOIN MasterEmployees me1 ON me1.EmployeeCode = d.ApproverId1
	INNER JOIN MasterEmployees me2 ON me2.EmployeeCode = d.ApproverId2
	INNER JOIN MasterEmployees me3 ON me3.EmployeeCode = d.ApproverId3
	WHERE c.AddendumContractId = @ID;
END
EXEC SP_GetAddendumContractByID @ID = 10;