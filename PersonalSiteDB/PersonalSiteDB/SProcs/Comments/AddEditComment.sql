CREATE PROCEDURE [dbo].[usp_AddEditComment]
	@Id INT = NULL,
	@UserId INT,
	@PostId INT,
	@InReplyTo INT,
	@Content VARCHAR(MAX),
	@InModeration BIT
AS
BEGIN

SET NOCOUNT ON

	IF EXISTS(SELECT 1 FROM Comments WHERE Id = @Id)
	BEGIN
		UPDATE Comments SET
			Content = @Content,
			LastModified = GETDATE()
		WHERE Id = @Id
	END
	ELSE 
	BEGIN
		INSERT INTO Comments(UserId, PostId, RepliesTo, Content, InModeration, DateCreated)
		VALUES
		(@UserId, @PostId, @InReplyTo, @Content, @InModeration, GETDATE())
	END

END