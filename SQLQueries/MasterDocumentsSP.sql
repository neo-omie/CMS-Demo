USE CMS_Trailblazers
-- Get All Documents
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

-- Get Document By ID
CREATE OR ALTER PROCEDURE SP_GetDocumentByID @id int
AS
BEGIN
	SELECT * FROM MasterDocuments
	WHERE ValueId = @id;
END
EXEC SP_GetDocumentByID @id = 1;

-- Delete Document
CREATE OR ALTER PROCEDURE SP_DeleteDocumentById @id int
As
Begin
	Update MasterDocuments
	set IsDeleted = 1
	where ValueId = @id;
End
EXEC SP_DeleteDocumentById @id=3

-- Add And Update Document
CREATE OR ALTER PROCEDURE SP_AddAndUpdateDocument 
@valueId int,
@documentName nvarchar(max),
@status int,
@documentType nvarchar(max),
@documentData varbinary(max),
@isDeleted int
As
Begin
	if @valueId is null
	begin
		Insert into MasterDocuments (DocumentName,status,DocumentType,DocumentData,IsDeleted) 
		Values (@documentName,@status,@documentType,@documentData,@isDeleted)
	End
	Else 
	Begin
		Update MasterDocuments
		Set DocumentName = @DocumentName,status = @Status,DocumentData = @documentData,DocumentType =@documentType
		Where ValueId = @valueId
	End
End
