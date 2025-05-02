-- Get Countries
CREATE OR ALTER PROCEDURE SP_GetCountries
AS
BEGIN
	SELECT * FROM Countries;
END
EXEC SP_GetCountries;

-- Get States
CREATE OR ALTER PROCEDURE SP_GetStates @CountryID int
AS
BEGIN
	SELECT * FROM States WHERE CountryId = @CountryID;
END
EXEC SP_GetStates @CountryID = 1;

-- Get Cities
CREATE OR ALTER PROCEDURE SP_GetCities @StateID int
AS
BEGIN
	SELECT * FROM Cities WHERE StateId = @StateID;
END
EXEC SP_GetCities @StateID = 1;
