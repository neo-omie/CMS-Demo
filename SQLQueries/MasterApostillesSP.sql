CREATE OR ALTER PROCEDURE SP_GetAllApostilles @PageNumber int, @PageSize int
AS
DECLARE @TotalRecords int
BEGIN
	SELECT @TotalRecords = COUNT(ValueId) FROM MasterApostilles;
	SELECT *, @TotalRecords as TotalRecords FROM MasterApostilles
	WHERE IsDeleted = 0
	ORDER BY ValueId
	OFFSET(@PageNumber-1)*@PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
EXEC SP_GetAllApostilles @PageNumber = 1, @PageSize = 10;

CREATE OR ALTER PROCEDURE SP_GetApostilleByID @id int
AS
BEGIN
	SELECT * FROM MasterApostilles
	WHERE ValueId = @id;
END
EXEC SP_GetApostilleByID @id = 1;