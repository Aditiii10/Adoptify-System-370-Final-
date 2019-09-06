CREATE PROCEDURE Don_SearchDon
      @Name NVARCHAR(30)
AS
BEGIN
      SET NOCOUNT ON;
      SELECT *
      FROM Donation_Type
      WHERE Donation_Type_Name LIKE '%' + @Name + '%' OR Donation_Type_Description LIKE '%' + @Name + '%' 
END