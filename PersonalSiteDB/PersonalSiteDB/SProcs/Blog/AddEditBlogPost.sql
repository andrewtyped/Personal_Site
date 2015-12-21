CREATE PROCEDURE [dbo].[usp_AddEditBlogPost]
	@Id INT = NULL,
	@Title VARCHAR(200),
	@Content VARCHAR(MAX),
	@DateCreated DATETIME,
	@TagTable TagType READONLY
AS
BEGIN
	IF (@Id IS NULL)
	BEGIN
		INSERT INTO BlogPosts(Title,DateCreated,Content)
		VALUES
		(@Title,@DateCreated,@Content)

		SELECT @Id = SCOPE_IDENTITY();
	END
	ELSE
	BEGIN
		UPDATE BlogPosts
		SET Title = @Title,
			Content = @Content
		WHERE Id = @Id
	END

	INSERT INTO Tags(Name)
	SELECT DISTINCT new.Name
	FROM @TagTable new
	LEFT JOIN Tags existing ON new.Name = existing.Name
	WHERE existing.Name IS NULL

	INSERT INTO BlogPostTags(BlogPostId,TagId)
	SELECT DISTINCT @Id, existing.Id
	FROM @TagTable new
	JOIN Tags  existing ON new.Name = existing.Name
END
GO