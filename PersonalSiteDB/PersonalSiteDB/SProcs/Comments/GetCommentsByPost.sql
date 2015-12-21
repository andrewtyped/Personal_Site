CREATE PROCEDURE [dbo].[usp_GetCommentsByPost]
	@PostId INT
AS
BEGIN

SET NOCOUNT ON

	SELECT Id, UserId, RepliesTo, Content, InModeration, DateCreated, LastModified
	FROM Comments
	WHERE PostId = @PostId
	ORDER BY DateCreated 

END