SELECT * FROM ContractsEntity
CREATE PROCEDURE SP_ContractsCounts
AS
BEGIN
	SELECT COUNT(*) AS AllContractsCount FROM ContractsEntity;
	SELECT COUNT(*) AS PendingApprovalContractsCount FROM ContractsEntity WHERE Approver3Status = 1;
	SELECT COUNT(*) AS PendingTerminationContractsCount FROM ContractsEntity WHERE Approver3Status = 6;
	SELECT COUNT(*) AS ExpiredContractsCount FROM ContractsEntity WHERE Approver3Status = 5;
	SELECT COUNT(*) AS ActiveContractsCount FROM ContractsEntity WHERE Approver3Status = 2;
	SELECT COUNT(*) AS TerminatedContractsCount FROM ContractsEntity WHERE Approver3Status = 4;
END
EXEC SP_ContractsCounts