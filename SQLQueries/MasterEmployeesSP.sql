USE CMS_Trailblazers
-- Get All Employees
CREATE OR ALTER PROCEDURE sp_GetAllEmployees @PageNumber int, @PageSize int, @Unit nvarchar(100)=null, @SearchTerm nvarchar(255)=null
AS
declare @totalCount int
BEGIN
	SELECT @totalCount = Count(ValueId) FROM MasterEmployees;
	SELECT *, @totalCount as TotalCount
	FROM MasterEmployees
	WHERE IsDeleted=0
	AND(Unit=@Unit OR @Unit is Null OR @Unit='All')
	AND(
	(@SearchTerm IS NOT NULL AND ValueId = TRY_CAST(@SearchTerm as int))
	OR
	(@SearchTerm IS NOT NULL AND EmployeeName LIKE '%' + @SearchTerm + '%')
	OR 
	(@SearchTerm IS NULL)
	)
	ORDER BY ValueId
	OFFSET(@PageNumber-1)*@PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
EXEC sp_GetAllEmployees @PageNumber= 1, @PageSize=10, @unit= 'All', @searchTerm= ''

-- Get Employee By ID
CREATE OR ALTER PROCEDURE sp_GetEmployeeById @Id int
AS
BEGIN
	SELECT * FROM MasterEmployees
	WHERE ValueId = @Id AND IsDeleted = 0
END
EXEC sp_GetEmployessById @Id=1

-- Add New Employee
CREATE OR ALTER PROCEDURE sp_AddEmployee 
	@EmployeeName nvarchar(100),
	@Password nvarchar(1000),
	@Role nvarchar(100),
	@EmployeeCode nvarchar(100),
    @Unit nvarchar(100),
    @DepartmentId int,
    @EmployeeMobile bigint,
    @Email nvarchar(255),
    @EmployeeExtension nvarchar(20),
    @LastPasswordChanged datetime,
	@IsDeleted BIT,
	@ValueId int Output
AS 
BEGIN
	INSERT INTO MasterEmployees (
			EmployeeName,
			Password,
			Role,
			EmployeeCode,
			Unit,
			DepartmentId,
			EmployeeMobile,
			Email,
			EmployeeExtension,
			LastPasswordChanged,
			IsDeleted
		)Values(
			@EmployeeName,
			@Password,
			@Role,
			@EmployeeCode,
			@Unit,
			@DepartmentId,
			@EmployeeMobile,
			@Email,
			@EmployeeExtension,
			@LastPasswordChanged,
			@IsDeleted
		)
		set @ValueId = SCOPE_IDENTITY();
END
EXEC sp_AddEmployee @EmployeeName='Light',
        @Password='Light@123',
        @Role='MOU_User',
        @EmployeeCode='NEO11',
        @Unit='Finance',
        @DepartmentId=1,
        @EmployeeMobile=4537878989,
        @Email='light123456@neosoft.com',
        @EmployeeExtension='IT Smart',
        @LastPasswordChanged= '2025-04-30 00:29:14.429',
		@IsDeleted=0
Select * from MasterEmployees

-- Delete Employee
CREATE OR ALTER PROCEDURE sp_DeleteEmployee
@Id int
AS
BEGIN
	DECLARE @CurrentDepartmentId INT;
	DECLARE @CurrentEmployeeCode NVARCHAR(100);

	SELECT @CurrentEmployeeCode = EmployeeCode
	FROM MasterEmployees
	WHERE ValueId=@Id;

    UPDATE MasterApprovalMatrixContracts
    SET ApproverId1 = 'NEO1'
    WHERE ApproverId1 = @CurrentEmployeeCode;

	UPDATE MasterApprovalMatrixContracts
    SET ApproverId2 = 'NEO1'
    WHERE ApproverId2 = @CurrentEmployeeCode;
	
	UPDATE MasterApprovalMatrixContracts
    SET ApproverId3 = 'NEO1'
    WHERE ApproverId3 = @CurrentEmployeeCode;

	UPDATE MasterApprovalMatrixMOUs
    SET ApproverId1 = 'NEO1'
    WHERE ApproverId1 = @CurrentEmployeeCode;

	UPDATE MasterApprovalMatrixMOUs
    SET ApproverId2 = 'NEO1'
    WHERE ApproverId2 = @CurrentEmployeeCode;
	
	UPDATE MasterApprovalMatrixMOUs
    SET ApproverId3 = 'NEO1'
    WHERE ApproverId3 = @CurrentEmployeeCode;

    UPDATE MasterEscalationMatrixMous
    SET EscalationId1 = 'NEO1'
    WHERE EscalationId1 = @CurrentEmployeeCode;

	UPDATE MasterEscalationMatrixMous
    SET EscalationId2 = 'NEO1'
    WHERE EscalationId2 = @CurrentEmployeeCode;
	
	UPDATE MasterEscalationMatrixMous
    SET EscalationId3 = 'NEO1'
    WHERE EscalationId3 = @CurrentEmployeeCode;

	UPDATE MasterEscalationMatrixContracts
    SET EscalationId1 = 'NEO1'
    WHERE EscalationId1 = @CurrentEmployeeCode;

	UPDATE MasterEscalationMatrixContracts
    SET EscalationId2 = 'NEO1'
    WHERE EscalationId2 = @CurrentEmployeeCode;
	
	UPDATE MasterEscalationMatrixContracts
    SET EscalationId3 = 'NEO1'
    WHERE EscalationId3 = @CurrentEmployeeCode;

	UPDATE MasterEmployees
	SET IsDeleted=1
	WHERE ValueId=@Id
END
EXEC sp_DeleteEmployee @Id=24

-- Update Employee
CREATE OR ALTER PROCEDURE sp_UpdateEmployee
	@Id INT,
    @EmployeeName NVARCHAR(255),
    @Password NVARCHAR(255),
    @Role NVARCHAR(100),
    @EmployeeCode NVARCHAR(100),
    @Unit NVARCHAR(100),
    @DepartmentId INT,
    @EmployeeMobile bigint,
    @Email NVARCHAR(255),
    @EmployeeExtension NVARCHAR(20)
AS
BEGIN
	DECLARE @CurrentDepartmentId INT;
	DECLARE @CurrentEmployeeCode NVARCHAR(100);
	SELECT @CurrentDepartmentId = DepartmentId, @CurrentEmployeeCode = EmployeeCode
	FROM MasterEmployees
	WHERE ValueId=@Id;

	IF @CurrentDepartmentId != @DepartmentId or @CurrentEmployeeCode != @EmployeeCode
	BEGIN
		UPDATE MasterApprovalMatrixContracts
		SET ApproverId1 = 'NEO1'
		WHERE ApproverId1 = @CurrentEmployeeCode and DepartmentId = @CurrentDepartmentId;

		UPDATE MasterApprovalMatrixContracts
		SET ApproverId2 = 'NEO1'
		WHERE ApproverId2 = @CurrentEmployeeCode and DepartmentId = @CurrentDepartmentId;
	
		UPDATE MasterApprovalMatrixContracts
		SET ApproverId3 = 'NEO1'
		WHERE ApproverId3 = @CurrentEmployeeCode and DepartmentId = @CurrentDepartmentId;

		UPDATE MasterApprovalMatrixMOUs
		SET ApproverId1 = 'NEO1'
		WHERE ApproverId1 = @CurrentEmployeeCode and DepartmentId = @CurrentDepartmentId;

		UPDATE MasterApprovalMatrixMOUs
		SET ApproverId2 = 'NEO1'
		WHERE ApproverId2 = @CurrentEmployeeCode and DepartmentId = @CurrentDepartmentId;
	
		UPDATE MasterApprovalMatrixMOUs
		SET ApproverId3 = 'NEO1'
		WHERE ApproverId3 = @CurrentEmployeeCode and DepartmentId = @CurrentDepartmentId;

		UPDATE MasterEscalationMatrixMous
		SET EscalationId1 = 'NEO1'
		WHERE EscalationId1 = @CurrentEmployeeCode and DepartmentId = @CurrentDepartmentId;

		UPDATE MasterEscalationMatrixMous
		SET EscalationId2 = 'NEO1'
		WHERE EscalationId2 = @CurrentEmployeeCode and DepartmentId = @CurrentDepartmentId;
	
		UPDATE MasterEscalationMatrixMous
		SET EscalationId3 = 'NEO1'
		WHERE EscalationId3 = @CurrentEmployeeCode and DepartmentId = @CurrentDepartmentId;

		UPDATE MasterEscalationMatrixContracts
		SET EscalationId1 = 'NEO1'
		WHERE EscalationId1 = @CurrentEmployeeCode and DepartmentId = @CurrentDepartmentId;

		UPDATE MasterEscalationMatrixContracts
		SET EscalationId2 = 'NEO1'
		WHERE EscalationId2 = @CurrentEmployeeCode and DepartmentId = @CurrentDepartmentId;
	
		UPDATE MasterEscalationMatrixContracts
		SET EscalationId3 = 'NEO1'
		WHERE EscalationId3 = @CurrentEmployeeCode and DepartmentId = @CurrentDepartmentId;
	END

	UPDATE MasterEmployees
	SET
		EmployeeName=@EmployeeName,
		Password=@Password,
		Role=@Role,
		EmployeeCode=@EmployeeCode,
		Unit=@Unit,
		DepartmentId=@DepartmentId,
		EmployeeMobile=@EmployeeMobile,
		Email=@Email,
		EmployeeExtension=@EmployeeExtension
	WHERE ValueId=@Id
END
EXEC sp_UpdateEmployee @Id=24, 
		@EmployeeName='Light1',
		@Password='Light@123',
        @Role='MOU_User',
        @EmployeeCode='NEO8',
        @Unit='Indore',
        @DepartmentId=2,
        @EmployeeMobile=4537878989,
        @Email='light@neosoft.com',
        @EmployeeExtension='IT Smart'

-- Get Employees By Department And Query
CREATE OR ALTER PROCEDURE sp_GetEmployeesByDepartmentAndQuery
@DepartmentId int,
@inpQuery nvarchar(100)
AS
BEGIN
	SELECT * FROM MasterEmployees
	WHERE DepartmentId=@DepartmentId
	AND (EmployeeName LIKE '%' + @inpQuery + '%' OR EmployeeCode LIKE '%' + @inpQuery + '%')
END
EXEC sp_GetEmployeesByDepartmentAndQuery @DepartmentId=2, @inpQuery='igh'
