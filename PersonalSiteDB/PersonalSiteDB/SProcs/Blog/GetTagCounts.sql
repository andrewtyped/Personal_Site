CREATE PROCEDURE [dbo].[usp_GetTagCounts]
AS
BEGIN
	SELECT Name, COUNT(Name) AS PostCount
	FROM BlogPostTags
	JOIN Tags ON TagId = Id
	GROUP BY Name
END
GO