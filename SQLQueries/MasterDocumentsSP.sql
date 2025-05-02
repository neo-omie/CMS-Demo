CREATE OR ALTER PROCEDURE SP_GetAllDocuments @PageNumber int, @PageSize int
AS
BEGIN
	SELECT * FROM MasterDocuments
	WHERE IsDeleted = 0
	ORDER BY ValueId
	OFFSET(@PageNumber-1)*@PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
EXEC SP_GetAllDocuments @PageNumber = 1, @PageSize = 10;

CREATE OR ALTER PROCEDURE SP_GetDocumentByID @id int
AS
BEGIN
	SELECT * FROM MasterDocuments
	WHERE ValueId = @id;
END
EXEC SP_GetDocumentByID @id = 1;