CREATE PROCEDURE [dbo].[usp_GetBlogStats]
AS
BEGIN
	SELECT
		MAX(Id) AS MaxPostId,
		MAX(DateCreated) AS MaxPostDate,
		MIN(Id) AS MinPostID, 
		MIN(DateCreated) AS MinPostDate,
		COUNT(Id) AS PostCount
	FROM
		BlogPosts posts
END
GO