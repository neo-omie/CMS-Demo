CREATE TRIGGER trg_Audit_MasterEmployee
ON MasterEmployee
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ActionDescription NVARCHAR(MAX);
    DECLARE @LoggedBy NVARCHAR(50); 
    DECLARE @Status NVARCHAR(50); 
    DECLARE @ForTable INT = 8; 

    SET @LoggedBy = SYSTEM_USER;

   
    IF EXISTS (SELECT * FROM inserted) AND NOT EXISTS (SELECT * FROM deleted)
    BEGIN
        SET @Status = 'Created'; 
        SET @ActionDescription = 'Inserted new record with ID: ' + CAST((SELECT ValueId FROM inserted) AS NVARCHAR);

        INSERT INTO AuditTrail (TableId, ForTable, ActionDescription, LogTime, LoggedBy, Status)
        SELECT 
            i.ValueId, 
            @ForTable, 
            @ActionDescription, 
            GETDATE(), 
            @LoggedBy, 
            1 
        FROM inserted i;
    END;

  
    IF EXISTS (SELECT * FROM deleted) AND NOT EXISTS (SELECT * FROM inserted)
    BEGIN
        SET @Status = 'Deleted'; 
        SET @ActionDescription = 'Deleted record with ID: ' + CAST((SELECT ValueId FROM deleted) AS NVARCHAR);

        INSERT INTO AuditTrail (TableId, ForTable, ActionDescription, LogTime, LoggedBy, Status)
        SELECT 
            d.ValueId, 
            @ForTable, 
            @ActionDescription, 
            GETDATE(), 
            @LoggedBy, 
            3 
        FROM deleted d;
    END;

   
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        SET @Status = 'Updated'; 
        SET @ActionDescription = (
            SELECT 
                'Updated record with ID: ' + CAST(i.Id AS NVARCHAR) + 
                '. Old values: { Name: ' + ISNULL(d.Name, 'NULL') + ', DepartmentId: ' + CAST(ISNULL(d.DepartmentId, 0) AS NVARCHAR) + ' }' +
                '. New values: { Name: ' + ISNULL(i.Name, 'NULL') + ', DepartmentId: ' + CAST(ISNULL(i.DepartmentId, 0) AS NVARCHAR) + ' }'
            FROM inserted i
            INNER JOIN deleted d ON i.ValueId = d.ValueId
        );

        INSERT INTO AuditTrail (TableId, ForTable, ActionDescription, LogTime, LoggedBy, Status)
        SELECT 
            i.ValueId, 
            @ForTable, 
            @ActionDescription, 
            GETDATE(), 
            @LoggedBy, 
            2 
        FROM inserted i
        INNER JOIN deleted d ON i.ValueId = d.ValueId;
    END;
END;