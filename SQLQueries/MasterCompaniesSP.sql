
--getting all companies
CREATE OR ALTER PROCEDURE SP_GetAllCompanies @PageNumber int, @PageSize int,@searchTerm nvarchar(50)
AS
DECLARE @TotalRecords int
BEGIN
	SELECT @TotalRecords = COUNT(ValueId) FROM MasterCompanies;
	SELECT m.ValueId, m.CompanyName, c.City as CompanyLocation, m.CompanyStatus as status, @TotalRecords as TotalRecords
	FROM MasterCompanies m
	LEFT JOIN Cities c ON c.CityId = m.CityId
	WHERE IsDeleted = 0 AND CompanyName LIKE ('%' + @searchTerm+ '%')
	ORDER BY ValueId
	OFFSET(@PageNumber-1)*@PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
END
EXEC SP_GetAllCompanies @PageNumber = 1, @PageSize = 10, @searchTerm='';

go

--deleting company by id
CREATE OR Alter PROCEDURE SP_DeleteCompanyById @ValId int
AS 
BEGIN
	DELETE FROM MasterCompanies
	WHERE ValueId=@ValId;
END
EXEC SP_DeleteCompanyById @ValId=1



--getting company by id 
CREATE OR ALTER PROCEDURE SP_GetCompanyByID @ValId int
AS 
BEGIN 
	SELECT * FROM MasterCompanies
	WHERE ValueId=@ValId;
END
EXEC SP_GetCompanyById @ValId=1

--Updating Company
CREATE OR ALTER PROCEDURE SP_UpdateCompany 
	@ValId int, @CompanyName nvarchar(255),
	@PocName nvarchar(255), @CompanyStatus bit,
	@PocContactNumber bigint,@PocEmailId nvarchar(255),
	@CompanyAddressLine1 nvarchar(255), @CompanyAddressLine2 nvarchar(255),
	@CompanyAddressLine3 nvarchar(255), @Zipcode int,
	@CompanyContactNo bigint, @CompanyEmailId nvarchar(255),
	@CompanyWebsiteUrl nvarchar(255), @CompanyBankName nvarchar(255),
	@GSTno bigint, @BankAccNo bigint,
	@MSMERegistrationNo bigint, @IFSCCode nvarchar(255), @PanNo nvarchar(255),
	@CountryId int, @StateId int, @CityId int
AS
BEGIN
	SELECT * FROM MasterCompanies
	WHERE ValueId=@ValId;
	UPDATE MasterCompanies
	SET CompanyName=@CompanyName,@PocName=@PocName,CompanyStatus=@CompanyStatus,PocContactNumber=@PocContactNumber,PocEmailId=@PocEmailId,
	CompanyAddressLine1=@CompanyAddressLine1,CompanyAddressLine2=@CompanyAddressLine2,CompanyAddressLine3=@CompanyAddressLine3,Zipcode=@Zipcode,
	CompanyContactNo=@CompanyContactNo,CompanyEmailId=@CompanyEmailId,CompanyWebsiteUrl=@CompanyWebsiteUrl,CompanyBankName=@CompanyBankName,
	GSTno=@GSTno ,BankAccNo=@BankAccNo, MSMERegistrationNo=@MSMERegistrationNo,IFSCCode=@IFSCCode,PanNo=@PanNo,
	CountryId=@CountryId, StateId=@StateId, CityId = @CityId
	WHERE ValueId=@ValId;
END	

EXEC SP_UpdateCompany @ValId=1, @CompanyName=null,
	@PocName =null, @CompanyStatus =null,
	@PocContactNumber=null,@PocEmailId =null,
	@CompanyAddressLine1 =null, @CompanyAddressLine2 =null,
	@CompanyAddressLine3 =null, @Zipcode=null,
	@CompanyContactNo=null, @CompanyEmailId =null,
	@CompanyWebsiteUrl =null, @CompanyBankName =null,
	@GSTno=null, @BankAccNo=null,
	@MSMERegistrationNo=null, @IFSCCode=null, @PanNo =null


--inserting values into company
CREATE PROCEDURE SP_AddCompany
@CompanyName NVARCHAR(255),
    @PocName NVARCHAR(255),
    @CompanyStatus BIT = 1,
    @PocContactNumber BIGINT,
    @PocEmailId NVARCHAR(255),
    @CompanyAddressLine1 NVARCHAR(255),
    @CompanyAddressLine2 NVARCHAR(255),
    @CompanyAddressLine3 NVARCHAR(255),
    @Zipcode INT,
    @CompanyContactNo BIGINT,
    @CompanyEmailId NVARCHAR(255),
    @CompanyWebsiteUrl NVARCHAR(255),
    @CompanyBankName NVARCHAR(255),
    @GSTno BIGINT,
    @BankAccNo BIGINT,
    @MSMERegistrationNo BIGINT,
    @IFSCCode NVARCHAR(50),
    @PanNo NVARCHAR(50),
    @IsDeleted BIT = 0,
    @CountryId INT,
    @StateId INT,
    @CityId INT
AS
BEGIN
    --SET NOCOUNT ON;
	INSERT INTO MasterCompanies (
        CompanyName,
        PocName,
        CompanyStatus,
        PocContactNumber,
        PocEmailId,
        CompanyAddressLine1,
        CompanyAddressLine2,
        CompanyAddressLine3,
        Zipcode,
        CompanyContactNo,
        CompanyEmailId,
        CompanyWebsiteUrl,
        CompanyBankName,
        GSTno,
        BankAccNo,
        MSMERegistrationNo,
        IFSCCode,
        PanNo,
        IsDeleted,
        CountryId,
        StateId,
        CityId
    )
    VALUES (
        @CompanyName,
        @PocName,
        @CompanyStatus,
        @PocContactNumber,
        @PocEmailId,
        @CompanyAddressLine1,
        @CompanyAddressLine2,
        @CompanyAddressLine3,
        @Zipcode,
        @CompanyContactNo,
        @CompanyEmailId,
        @CompanyWebsiteUrl,
        @CompanyBankName,
        @GSTno,
        @BankAccNo,
        @MSMERegistrationNo,
        @IFSCCode,
        @PanNo,
        @IsDeleted,
        @CountryId,
        @StateId,
        @CityId
    );
END;
	


