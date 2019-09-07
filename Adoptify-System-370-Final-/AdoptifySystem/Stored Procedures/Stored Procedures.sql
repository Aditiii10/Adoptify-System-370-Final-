--CREATE PROCEDURE Don_SearchDon
--      @Name NVARCHAR(30)
--AS
--BEGIN
--      SET NOCOUNT ON;
--      SELECT *
--      FROM Donation_Type
--      WHERE Donation_Type_Name LIKE '%' + @Name + '%' OR Donation_Type_Description LIKE '%' + @Name + '%' 
--END

--This is For Search Donor
--CREATE PROCEDURE Donor_SearchDonor
--      @Name NVARCHAR(30)
--AS
--BEGIN
--      SET NOCOUNT ON;
--      SELECT *
--      FROM Donor
--      WHERE Donor_Name LIKE '%' + @Name + '%' OR Donor_Surname LIKE '%' + @Name + '%' OR Donor_Email LIKE '%' + @Name + '%'
--END

-- This is Search For Animal Type
--CREATE PROCEDURE AnimalType_SearchAnimalType
--      @Name NVARCHAR(30)
--AS
--BEGIN
--      SET NOCOUNT ON;
--      SELECT *
--      FROM Animal_Type
--      WHERE Animal_Type_Name LIKE '%' + @Name + '%'
--END
-- This is Search For Breed Type
--CREATE PROCEDURE BreedType_SearchBreedType
--      @Name NVARCHAR(30)
--AS
--BEGIN
--      SET NOCOUNT ON;
--      SELECT *
--      FROM Animal_Breed
--      WHERE Animal_Breed_Description LIKE '%' + @Name + '%' OR Animal_Breed_Name LIKE '%' + @Name + '%'
--END
-- This is Search For Foster Care Parent
--CREATE PROCEDURE Parent_SearchParent
--      @Name NVARCHAR(30)
--AS
--BEGIN
--      SET NOCOUNT ON;
--      SELECT *
--      FROM Foster_Care_Parent
--      WHERE Foster_Parent_Name LIKE '%' + @Name + '%' OR Foster_Parent_Surname LIKE '%' + @Name + '%' OR Foster_Parent_Email LIKE '%' + @Name + '%'
--END
-- This is Search For Employee Type
--CREATE PROCEDURE Emp_SearchEmpType
--      @Name NVARCHAR(30)
--AS
--BEGIN
--      SET NOCOUNT ON;
--      SELECT *
--      FROM Employee_Type
--      WHERE Emp_Type_Name LIKE '%' + @Name + '%' OR Emp_Type_Description LIKE '%' + @Name + '%' 
--END
--this is search for Stock
--CREATE PROCEDURE Stock_SearchStock
--      @Name NVARCHAR(30)
--AS
--BEGIN
--      SET NOCOUNT ON;
--SELECT *
--FROM Stock as a inner join Stock_Type as b on a.Stock_Type_ID = b.Stock_Type_ID 
--WHERE a.Stock_Description LIKE '%' + @Name + '%' OR b.Stock_Type_Name LIKE '%' + @Name + '%' OR b.Stock_Type_Description LIKE '%' + @Name + '%'
--END
--this is search for Stock Type
CREATE PROCEDURE Stock_SearchStockType
      @Name NVARCHAR(30)
AS
BEGIN
      SET NOCOUNT ON;
      SELECT *
      FROM Stock_Type
      WHERE Stock_Type_Name LIKE '%' + @Name + '%' OR Stock_Type_Description LIKE '%' + @Name + '%' 
END



--Still need to complete

--CREATE PROCEDURE Don_SearchDon
--      @Name NVARCHAR(30)
--AS
--BEGIN
--      SET NOCOUNT ON;
--      SELECT *
--      FROM Donation_Type
--      WHERE Donation_Type_Name LIKE '%' + @Name + '%' OR Donation_Type_Description LIKE '%' + @Name + '%' 
--END