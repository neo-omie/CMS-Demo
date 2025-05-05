CREATE OR ALTER PROCEDURE SP_GetAllClassifiedContracts @PageNumber int, @PageSize int
AS
DECLARE @TotalRecords int
BEGIN
	SELECT @TotalRecords = COUNT(ClassifiedContractId) FROM ClassifiedContracts

	SELECT c.ClassifiedContractId as ClassifiedContractId, c.ClassifiedContractName as ClassifiedContractName,
	cc.ContractTypeName as ContractType, dd.DepartmentName as DepartmentName,
	c.ValidFrom as EffectiveDate, c.ValidTill as ExpiryDate,
	c.RenewalFrom as ToBeRenewedOn, c.RenewalTill as AddendumDate,
	c.Approver3Status as Status, me.EmployeeName as ApprovalPendingFrom,
	me.EmployeeName as RenewalContractPerson, CAST(CAST((CAST(c.RenewalTill as datetime) - GETDATE()) as int) as nvarchar(50)) as RenewalDueIn,
	c.Location as Location, @TotalRecords as TotalRecords
	FROM ClassifiedContracts c
	LEFT JOIN contracts cc ON cc.ValueId = c.ContractTypeId
	LEFT JOIN MasterApprovalMatrixContracts d ON d.DepartmentId = c.DepartmentId
	LEFT JOIN Departments dd ON dd.DepartmentId = c.DepartmentId
	LEFT JOIN MasterEmployees me ON me.EmployeeCode = d.ApproverId3
	WHERE c.IsDeleted = 0
	ORDER BY c.ClassifiedContractId
	OFFSET(@PageNumber-1)*@PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END

