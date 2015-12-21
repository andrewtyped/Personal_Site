CREATE PROCEDURE [dbo].[usp_GetOlderPosts]
	@MaxID INT,
	@NumResults INT,
	@ReturnContent BIT = 0
AS
BEGIN
	SELECT TOP(@NumResults) 
		Id,
		Title,
		DateCreated,
		CASE
			WHEN @ReturnContent = 1 THEN Content
			ELSE ''
		END AS Content
	FROM
		BlogPosts
	WHERE Id <= @MaxID
	ORDER BY DateCreated DESC
END
GO