USE CMS_Trailblazers
-- Get All
CREATE OR ALTER PROCEDURE SP_GetAllContractsEntity @PageNumber int, @PageSize int,
@SearchTerm nvarchar(200), @FromDate date, @ToDate date, @ContractType int,
@RenewalDueIn int, @ContractStatus int, @Department int, @Location nvarchar(100),
@HasAddendum bit
AS
DECLARE @TotalRecords int
BEGIN
	IF(@HasAddendum = 1)
	BEGIN
		SELECT @TotalRecords = COUNT(ContractId) FROM ContractsEntity WHERE IsDeleted=0
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
		WHERE c.IsDeleted = 0 AND
		((@SearchTerm IS NOT NULL AND c.ContractName LIKE '%' + @SearchTerm + '%') OR (@SearchTerm IS NULL)) AND
		((@FromDate IS NOT NULL AND c.ValidFrom >= @FromDate) OR (@FromDate IS NULL)) AND
		((@ToDate IS NOT NULL AND c.ValidTill <= @ToDate) OR (@ToDate IS NULL)) AND
		((@ContractType IS NOT NULL AND c.ContractTypeId = @ContractType) OR (@ContractType IS NULL)) AND
		((@RenewalDueIn IS NOT NULL AND c.RenewalFrom < DATEADD(day, @RenewalDueIn, CONVERT(DATE,GETDATE()))) OR (@RenewalDueIn IS NULL)) AND
		((@ContractStatus IS NOT NULL AND c.Approver3Status = @ContractStatus) OR (@ContractStatus IS NULL)) AND
		((@Department IS NOT NULL AND c.DepartmentId = @Department) OR (@Department IS NULL)) AND
		((@Location IS NOT NULL AND c.Location = @Location) OR (@Location IS NULL)) AND
		EXISTS (SELECT 1 FROM AddendumContracts ac WHERE ac.ContractId = c.ContractId)
		ORDER BY c.ContractId
		OFFSET(@PageNumber-1)*@PageSize ROWS
		FETCH NEXT @PageSize ROWS ONLY
	END
	ELSE IF(@HasAddendum = 0)
	BEGIN
		SELECT @TotalRecords = COUNT(ContractId) FROM ContractsEntity WHERE IsDeleted=0
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
		WHERE c.IsDeleted = 0 AND
		((@SearchTerm IS NOT NULL AND c.ContractName LIKE '%' + @SearchTerm + '%') OR (@SearchTerm IS NULL)) AND
		((@FromDate IS NOT NULL AND c.ValidFrom >= @FromDate) OR (@FromDate IS NULL)) AND
		((@ToDate IS NOT NULL AND c.ValidTill <= @ToDate) OR (@ToDate IS NULL)) AND
		((@ContractType IS NOT NULL AND c.ContractTypeId = @ContractType) OR (@ContractType IS NULL)) AND
		((@RenewalDueIn IS NOT NULL AND c.RenewalFrom < DATEADD(day, @RenewalDueIn, CONVERT(DATE,GETDATE()))) OR (@RenewalDueIn IS NULL)) AND
		((@ContractStatus IS NOT NULL AND c.Approver3Status = @ContractStatus) OR (@ContractStatus IS NULL)) AND
		((@Department IS NOT NULL AND c.DepartmentId = @Department) OR (@Department IS NULL)) AND
		((@Location IS NOT NULL AND c.Location = @Location) OR (@Location IS NULL)) AND
		NOT EXISTS (SELECT 1 FROM AddendumContracts ac WHERE ac.ContractId = c.ContractId)
		ORDER BY c.ContractId
		OFFSET(@PageNumber-1)*@PageSize ROWS
		FETCH NEXT @PageSize ROWS ONLY
	END
	ELSE BEGIN
		SELECT @TotalRecords = COUNT(ContractId) FROM ContractsEntity WHERE IsDeleted=0
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
		WHERE c.IsDeleted = 0 AND
		((@SearchTerm IS NOT NULL AND c.ContractName LIKE '%' + @SearchTerm + '%') OR (@SearchTerm IS NULL)) AND
		((@FromDate IS NOT NULL AND c.ValidFrom >= @FromDate) OR (@FromDate IS NULL)) AND
		((@ToDate IS NOT NULL AND c.ValidTill <= @ToDate) OR (@ToDate IS NULL)) AND
		((@ContractType IS NOT NULL AND c.ContractTypeId = @ContractType) OR (@ContractType IS NULL)) AND
		((@RenewalDueIn IS NOT NULL AND c.RenewalFrom < DATEADD(day, @RenewalDueIn, CONVERT(DATE,GETDATE()))) OR (@RenewalDueIn IS NULL)) AND
		((@ContractStatus IS NOT NULL AND c.Approver3Status = @ContractStatus) OR (@ContractStatus IS NULL)) AND
		((@Department IS NOT NULL AND c.DepartmentId = @Department) OR (@Department IS NULL)) AND
		((@Location IS NOT NULL AND c.Location = @Location) OR (@Location IS NULL))
		ORDER BY c.ContractId
		OFFSET(@PageNumber-1)*@PageSize ROWS
		FETCH NEXT @PageSize ROWS ONLY
	END
END
EXEC SP_GetAllContractsEntity 1, 10, 'Contract', '2025/03/03', '2025/09/06', null, null, null, null, null, 1;
EXEC SP_GetAllContractsEntity 1, 10, null, null, null, null, null, null, null, null, 0;
-- EXEC SP_GetAllContractsEntity @PageNumber = 1, @PageSize = 10;

SELECT CAST(CAST((CAST(c.RenewalTill as datetime) - GETDATE()) as int) as nvarchar(50)) as RenewalDueIn FROM ContractsEntity c;
SELECT CAST(RenewalTill as datetime) FROM ContractsEntity;

-- Get By ID
CREATE PROCEDURE SP_GetContractEntityByID @ID int
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
	c.EmpCustodianId as EmpCustodianId, me.EmployeeName as EmpCustodianName, me.Email as EmpCustodianEmail,
	me.EmployeeCode as EmpCustodianCode,
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
EXEC SP_GetContractEntityByID @ID = 3;

SELECT CAST(CAST((CAST(c.RenewalTill as datetime) - GETDATE()) as int) as nvarchar(50)) as RenewalDueIn FROM ContractsEntity c;
SELECT CAST(RenewalTill as datetime) FROM ContractsEntity;

-- Active Contracts
CREATE PROCEDURE SP_GetActiveContractsEntity @PageNumber int, @PageSize int
AS
DECLARE @TotalRecords int
BEGIN
	SELECT @TotalRecords = COUNT(ContractId) FROM ContractsEntity WHERE IsDeleted=0

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
	WHERE c.IsDeleted = 0 AND c.Approver3Status = 2
	ORDER BY c.ContractId
	OFFSET(@PageNumber-1)*@PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
EXEC SP_GetActiveContractsEntity @PageNumber = 1, @PageSize = 10;

-- Terminated Contracts
CREATE PROCEDURE SP_GetTerminatedContractsEntity @PageNumber int, @PageSize int
AS
DECLARE @TotalRecords int
BEGIN
	SELECT @TotalRecords = COUNT(ContractId) FROM ContractsEntity WHERE IsDeleted=0

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
	WHERE c.IsDeleted = 0 AND c.Approver3Status = 4
	ORDER BY c.ContractId
	OFFSET(@PageNumber-1)*@PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
EXEC SP_GetTerminatedContractsEntity @PageNumber = 1, @PageSize = 10;

-- Pending Approval Contracts
CREATE PROCEDURE SP_GetPendingApprovalContractsEntity @PageNumber int, @PageSize int
AS
DECLARE @TotalRecords int
BEGIN
	SELECT @TotalRecords = COUNT(ContractId) FROM ContractsEntity WHERE IsDeleted=0

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
	WHERE c.IsDeleted = 0 AND c.Approver3Status = 1
	ORDER BY c.ContractId
	OFFSET(@PageNumber-1)*@PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
EXEC SP_GetPendingApprovalContractsEntity @PageNumber = 1, @PageSize = 10;

-- Expired Contracts
CREATE PROCEDURE SP_GetExpiredContractsEntity @PageNumber int, @PageSize int
AS
DECLARE @TotalRecords int
BEGIN
	SELECT @TotalRecords = COUNT(ContractId) FROM ContractsEntity WHERE IsDeleted=0

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
	WHERE c.IsDeleted = 0 AND c.Approver3Status = 5
	ORDER BY c.ContractId
	OFFSET(@PageNumber-1)*@PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
EXEC SP_GetExpiredContractsEntity @PageNumber = 1, @PageSize = 10;

-- Get Contract By Name
CREATE PROCEDURE SP_GetContractEntityByName @Name nvarchar(100)
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
	c.EmpCustodianId as EmpCustodianId, me.EmployeeName as EmpCustodianName, me.Email as EmpCustodianEmail,
	me.EmployeeCode as EmpCustodianCode,
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
	WHERE c.ContractName = @Name;
END
