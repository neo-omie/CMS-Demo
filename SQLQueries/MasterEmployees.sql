Create or alter procedure sp_GetAllEmployees @PageNumber int, @PageSize int, @Unit nvarchar(100)=null, @SearchTerm nvarchar(255)=null
as 
declare @totalCount int
begin
Select @totalCount = Count(ValueId) from MasterEmployees
select *, @totalCount as TotalCount
from MasterEmployees
where IsDeleted=0
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
end
go

exec sp_GetAllEmployees @PageNumber= 1, @PageSize=10, @unit= 'All', @searchTerm= ''
go

create or alter procedure sp_GetEmployeeById @Id int
as 
begin
SELECT * FROM MasterEmployees
WHERE ValueId = @Id AND IsDeleted = 0
end 
go

exec sp_GetEmployessById @Id=1
go


create or alter procedure sp_AddEmployee 
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
Begin
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
end
go

exec sp_AddEmployee @EmployeeName='Light',
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
go

select * from MasterEmployees
go


create or alter procedure sp_DeleteEmployee
@Id int
as 
Begin 
	Update MasterEmployees
	Set IsDeleted=1
	where ValueId=@Id
end
go 

Exec sp_DeleteEmployee @Id=24
go


create or alter procedure sp_UpdateEmployee
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
as 
Begin 
Update MasterEmployees
set
	EmployeeName=@EmployeeName,
    Password=@Password,
    Role=@Role,
    EmployeeCode=@EmployeeCode,
    Unit=@Unit,
    DepartmentId=@DepartmentId,
    EmployeeMobile=@EmployeeMobile,
    Email=@Email,
    EmployeeExtension=@EmployeeExtension
where ValueId=@Id
end 
go

exec sp_UpdateEmployee @Id=24, 
		@EmployeeName='Light1',
		@Password='Light@123',
        @Role='MOU_User',
        @EmployeeCode='NEO8',
        @Unit='Indore',
        @DepartmentId=2,
        @EmployeeMobile=4537878989,
        @Email='light@neosoft.com',
        @EmployeeExtension='IT Smart'
go


create or alter procedure sp_GetEmployeesByDepartmentAndQuery
@DepartmentId int,
@inpQuery nvarchar(100)
as 
Begin
Select * from MasterEmployees
where DepartmentId=@DepartmentId
and (EmployeeName Like '%' + @inpQuery + '%' OR EmployeeCode Like '%' + @inpQuery + '%')
end 
go

EXEC sp_GetEmployeesByDepartmentAndQuery @DepartmentId=2, @inpQuery='igh'
go




