CREATE PROCEDURE [dbo].[usp_GetUserByName]
	@Name VARCHAR(100)
AS
BEGIN

	SELECT 
		u.Id,
		u.Name,
		u.Email,
		u.[Password],
		r.Name AS [Role]
	FROM [User] u
	JOIN [Role] r ON r.Id = u.RoleId
	WHERE u.Name = @Name
END

GO