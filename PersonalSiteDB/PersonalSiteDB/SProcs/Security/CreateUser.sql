CREATE PROCEDURE [dbo].[usp_CreateUser]
	@Name VARCHAR(50),
	@Email VARCHAR(100),
	@Password VARCHAR(800),
	@Role VARCHAR(30)
AS
BEGIN
	BEGIN TRY
	BEGIN TRAN
		
		DECLARE @RoleId INT

		SELECT
			@RoleId = ISNULL(Id,0)
		FROM 
			[Role]
		WHERE
			Name = @Role


		INSERT INTO 
			[User](Name,Email,[Password],RoleId)
		VALUES
			(@Name,@Email,@Password,@RoleId)

		SELECT SCOPE_IDENTITY()

	COMMIT
	END TRY
	BEGIN CATCH

		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK
		END

		SELECT ERROR_MESSAGE()

	END CATCH
END

GO