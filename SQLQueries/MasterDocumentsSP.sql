use CMS_Trailblazers
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

CREATE OR ALTER PROCEDURE SP_AddDocument 
@documentName nvarchar(max),
@status int,
@documentType nvarchar(max),
@documentData varbinary(max),
@isDeleted int
As
Begin
	Insert into MasterDocuments (DocumentName,status,DocumentType,IsDeleted) values (@documentName,@status,@isDeleted)
End
Exec SP_AddDocument @documentName='Resume',@status=1 ,@isDeleted =0

CREATE OR ALTER PROCEDURE SP_DeleteDocumentById @id int
As
Begin
	Update  MasterDocuments
	set IsDeleted = 1
	where ValueId = @id;
End
Exec SP_DeleteDocumentById  @id=3

CREATE OR ALTER PROCEDURE SP_UpdateDocumentById
@id int,
@DocumentName nvarchar(255),
@Status int
as
Begin
	update MasterDocuments
	set DocumentName = @DocumentName,status = @Status
	where	 ValueId =@id
End

Exec SP_UpdateDocumentById  @id=3 ,@DocumentName ='Addmission form',@Status=1
