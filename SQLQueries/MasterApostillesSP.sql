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
go

EXEC SP_GetAllApostilles @PageNumber = 1, @PageSize = 10;
go

CREATE OR ALTER PROCEDURE SP_GetApostilleByID @id int
AS
BEGIN
	SELECT * FROM MasterApostilles
	WHERE ValueId = @id;
END
go

EXEC SP_GetApostilleByID @id = 1;
go

Create or alter procedure sp_AddApostille 
	@ApostilleName nvarchar(100),
	@Status BIT,
	@IsDeleted BIT,
	@ValueId int Output
as
Begin 
Insert into MasterApostilles(
	ApostilleName,
	Status,
	IsDeleted
)Values(
	@ApostilleName,
	@Status,
	@IsDeleted
)
set @ValueId=SCOPE_IDENTITY();
END
GO

Exec sp_AddApostille @ApostilleName='Shinchan', @Status=1, @IsDeleted=1
go	

create or alter procedure sp_DeleteApostille
@Id int 
as 
Begin
	Update MasterApostilles
	set IsDeleted=1
	where ValueId=@Id
End
go

EXEC sp_DeleteApostille @Id=2
go

create or alter procedure sp_UpdateApostille
@Id int, 
@ApostilleName nvarchar(100),
@Status Bit
as 
Begin
	Update MasterApostilles
	set 
		ApostilleName=@ApostilleName,
		Status=@Status
	Where ValueId=@Id
End
go

EXEC sp_UpdateApostille @Id=3, @ApostilleName='Shinchan', @Status=1
go