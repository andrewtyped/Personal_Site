CREATE PROCEDURE [dbo].[usp_DeleteBlogPost]
	@ID INT
AS
	DELETE FROM BlogPosts 
	WHERE Id = @ID

	DELETE FROM BlogPostTags
	WHERE BlogPostId = @Id
GO