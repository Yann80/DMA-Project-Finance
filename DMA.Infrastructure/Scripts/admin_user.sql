USE [MarketingTools]
GO
INSERT [dbo].[Users] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'af1f7474-5bc4-43f2-a237-e3cc8322f56c', N'admin', N'ADMIN', N'admin@externall.agency', N'ADMIN@EXTERNALL.AGENCY', 0, N'AQAAAAEAACcQAAAAEG4SIhvQ7vfNOhYwuud/p8WyyPaIAF/GAjFWO4yddPHBo7C45i7z4yBb/aUej8KS9Q==', N'JMT6O7UC45BZCMLLANFYELX43PSM72HA', N'3334d945-43c6-4a0c-93e6-fb6b323938fe', NULL, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[Roles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'17c7c859-3536-4b3a-847e-c231e7ddfebb', N'User', N'USER', NULL)
INSERT [dbo].[Roles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'57a16855-6359-4af8-b5fb-c967a422cf17', N'Administrator', N'ADMINISTRATOR', NULL)
GO
INSERT [dbo].[UserRoles] ([UserId], [RoleId]) VALUES (N'af1f7474-5bc4-43f2-a237-e3cc8322f56c', N'57a16855-6359-4af8-b5fb-c967a422cf17')
GO
