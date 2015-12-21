CREATE PROCEDURE [dbo].[usp_GetBlogPostsByTag]
	@TagName VARCHAR(30)
AS
BEGIN
	SELECT
		posts.Id,
		posts.Title,
		posts.DateCreated
	FROM
		BlogPosts posts
	LEFT JOIN BlogPostTags postTags ON postTags.BlogPostId = posts.Id
	LEFT JOIN Tags tag on tag.Id = postTags.TagId
	WHERE tag.Name LIKE LTRIM(RTRIM(@TagName))
	GROUP BY posts.Id,posts.Title,posts.DateCreated
	ORDER BY DateCreated DESC
END
GO