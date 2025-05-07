CREATE OR ALTER PROCEDURE SP_GetAllContractsTypes @PageNumber int, @PageSize int
AS
BEGIN
	SELECT * FROM contracts
	WHERE IsDeleted = 0 
	ORDER BY ValueId
	OFFSET(@PageNumber-1)*@PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
EXEC SP_GetAllContractsTypes @PageNumber = 1, @PageSize = 10;

go 

--addContract
CREATE PROCEDURE SP_AddContractType
    @ContractTypeName NVARCHAR(255),
	@Status bit
AS
BEGIN
    --SET NOCOUNT ON;
	INSERT INTO contracts(
       ContractTypeName,
	   Status,
	   IsDeleted
    )
    VALUES (
     @ContractTypeName,
	 @Status,
	 0
    );
END;

Exec SP_AddContractType @ContractTypeName='CSMR',@Status=1


--deleting company by id
CREATE OR Alter PROCEDURE SP_DeleteContractyId @ValId int
AS 
BEGIN
	update contracts
	set IsDeleted = 1
	WHERE ValueId=@ValId;
END
EXEC SP_DeleteContractById @ValId=1