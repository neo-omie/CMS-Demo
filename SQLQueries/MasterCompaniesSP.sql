
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


--getting company by id 
CREATE OR ALTER PROCEDURE SP_GetCompanyByID @ValId int
AS 
BEGIN 
	SELECT * FROM MasterCompanies
	WHERE ValueId=@ValId;
END
EXEC SP_GetCompanyById @ValId=1


