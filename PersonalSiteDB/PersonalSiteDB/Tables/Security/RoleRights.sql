CREATE TABLE [dbo].[RoleRights]
(
	[RoleId] INT FOREIGN KEY REFERENCES [dbo].[Role](Id),
	[RightId] INT FOREIGN KEY REFERENCES [dbo].[Right](Id)
)
