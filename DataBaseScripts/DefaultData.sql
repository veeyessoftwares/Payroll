USE [Payroll]
GO
SET IDENTITY_INSERT [dbo].[WAGESTYPE] ON 

INSERT [dbo].[WAGESTYPE] ([WGId], [Type], [IsActive], [IsDeleted]) VALUES (1, N'MONTHLY
', 1, 0)
INSERT [dbo].[WAGESTYPE] ([WGId], [Type], [IsActive], [IsDeleted]) VALUES (2, N'WEEKLY
', 1, 0)
SET IDENTITY_INSERT [dbo].[WAGESTYPE] OFF
