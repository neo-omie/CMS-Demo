CREATE OR ALTER PROCEDURE SP_GetAllCompanies @PageNumber int, @PageSize int,@searchTerm nvarchar(50)
AS
BEGIN
	SELECT * FROM MasterCompanies
	WHERE IsDeleted = 0 AND CompanyName LIKE ('%' + @searchTerm+ '%')
	ORDER BY ValueId
	OFFSET(@PageNumber-1)*@PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
EXEC SP_GetAllCompanies @PageNumber = 1, @PageSize = 10, @searchTerm='shrzaxde';