CREATE PROCEDURE [dbo].[usp_GetBlogPost]
	@ID INT = NULL
AS
BEGIN
	SELECT post.Id,Title,Content,DateCreated, ISNULL(tag.Name,'') as Name
	FROM BlogPosts post
	LEFT JOIN BlogPostTags postTags ON postTags.BlogPostId = post.Id
	LEFT JOIN Tags tag on tag.Id = postTags.TagId
	WHERE post.Id = @ID
END
GO