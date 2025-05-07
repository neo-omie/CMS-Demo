CREATE or alter PROCEDURE SP_GetAllDepartments @PageNumber int, @PageSize int
AS
DECLARE @TotalRecords int
BEGIN
	SELECT @TotalRecords = COUNT(DepartmentId) FROM Departments;
	SELECT *, @TotalRecords as TotalRecords FROM Departments
	ORDER BY DepartmentId
	OFFSET(@PageNumber-1)*@PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
EXEC SP_GetAllDepartments @PageNumber = 1, @PageSize = 10;

CREATE or alter PROCEDURE SP_GetDepartmentByID @id int
AS
BEGIN
	SELECT * FROM Departments
	WHERE DepartmentId = @id;
END
EXEC SP_GetDepartmentByID @id = 1;