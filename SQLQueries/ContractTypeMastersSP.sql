CREATE OR ALTER PROCEDURE SP_GetAllContracts @PageNumber int, @PageSize int
AS
BEGIN
	SELECT * FROM contracts
	WHERE IsDeleted = 0 
	ORDER BY ValueId
	OFFSET(@PageNumber-1)*@PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
EXEC SP_GetAllContracts @PageNumber = 1, @PageSize = 10;