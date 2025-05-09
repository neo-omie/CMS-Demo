CREATE PROCEDURE SP_GetAllEscalationMatrixContracts @PageNumber INT, @PageSize INT
AS
BEGIN
DECLARE @TotalRecords INT
	SELECT @TotalRecords = Count(m.MatrixContractId) FROM MasterEscalationMatrixContracts m; 
    SELECT 
        m.MatrixContractId,
        d.DepartmentName, m.DepartmentId,
        e1.EmployeeName AS Escalation1,m.EscalationId1,
        e2.EmployeeName AS Escalation2,m.EscalationId2,
        e3.EmployeeName AS Escalation3,m.EscalationId3,
		m.TriggerDaysEscalation1, m.TriggerDaysEscalation2, m.TriggerDaysEscalation3
    FROM MasterEscalationMatrixContracts m
    LEFT JOIN Departments d ON m.DepartmentId = d.DepartmentId
    LEFT JOIN MasterEmployees e1 ON m.EscalationId1 = e1.EmployeeCode
    LEFT JOIN MasterEmployees e2 ON m.EscalationId2 = e2.EmployeeCode
    LEFT JOIN MasterEmployees e3 ON m.EscalationId3 = e3.EmployeeCode
    ORDER BY m.MatrixContractId
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
EXEC SP_GetAllEscalationMatrixContracts @PageNumber = 1, @PageSize = 10;