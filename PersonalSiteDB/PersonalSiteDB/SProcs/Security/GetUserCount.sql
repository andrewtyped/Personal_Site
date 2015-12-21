CREATE PROCEDURE [dbo].[usp_GetUserCount]
AS
BEGIN

	SELECT 
		COUNT(Id)
	FROM 
		[User]
END

GO