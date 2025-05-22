USE CMS_Trailblazers
-- Get All and types of Contracts counts
CREATE OR ALTER PROCEDURE SP_ContractsCounts
AS
BEGIN
	-- Declaration
	declare @AllContractsCount int
	declare @PendingApprovalContractsCount int
	declare @PendingTerminationContractsCount int
	declare @ExpiredContractsCount int
	declare @ActiveContractsCount int
	declare @TerminatedContractsCount int

	-- Initialization
	SELECT @AllContractsCount = COUNT(*) FROM ContractsEntity;
	SELECT @PendingApprovalContractsCount = COUNT(*) FROM ContractsEntity WHERE Approver3Status = 1;
	SELECT @PendingTerminationContractsCount = COUNT(*) FROM ContractsEntity WHERE Approver3Status = 6;
	SELECT @ExpiredContractsCount = COUNT(*) FROM ContractsEntity WHERE Approver3Status = 5;
	SELECT @ActiveContractsCount = COUNT(*) FROM ContractsEntity WHERE Approver3Status = 2;
	SELECT @TerminatedContractsCount = COUNT(*) FROM ContractsEntity WHERE Approver3Status = 4;

	-- Execution and Output Display
	SELECT @AllContractsCount AS AllContractsCount,
		   @PendingApprovalContractsCount AS PendingApprovalContractsCount,
		   @PendingTerminationContractsCount AS PendingTerminationContractsCount,
		   @ExpiredContractsCount AS ExpiredContractsCount,
		   @ActiveContractsCount AS ActiveContractsCount,
		   @TerminatedContractsCount AS TerminatedContractsCount
END
EXEC SP_ContractsCounts
SELECT * FROM ContractsEntity