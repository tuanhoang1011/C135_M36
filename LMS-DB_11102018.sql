IF EXISTS(SELECT NAME FROM SYS.DATABASES WHERE NAME = 'LogManagementSystem')
DROP DATABASE LogManagementSystem
GO

CREATE DATABASE LogManagementSystem
GO

Use LogManagementSystem
GO

IF OBJECT_ID('[Role]','U') IS NOT NULL DROP TABLE [Role]
GO

CREATE TABLE [Role](
	RoleId INT IDENTITY(1,1) PRIMARY KEY,
	RoleName VARCHAR(15) NOT NULL,
	[Permissions] INT NOT NULL,
	CreateDate DATETIME,
	ModifyDate DATETIME,
	IsDeleted BIT
)
GO

IF OBJECT_ID('[User]','U') IS NOT NULL DROP TABLE [User]
GO

CREATE TABLE [User](
	UserId INT IDENTITY(1,1) PRIMARY KEY,
	UserName VARCHAR(20) NOT NULL UNIQUE,
	[Password] VARCHAR(20) NOT NULL,
	FullName NVARCHAR(35) NOT NULL,
	Birthdate DATE,
	Gender BIT,
	Phone VARCHAR(15),
	[Address] NVARCHAR(100),
	CreateDate DATETIME,
	ModifyDate DATETIME,
	RoleId INT NOT NULL,
	FOREIGN KEY(RoleId) REFERENCES [Role](RoleId),
	IsLoggedIn BIT,
	IsDeleted BIT
)
GO

IF OBJECT_ID('[Log]','U') IS NOT NULL DROP TABLE [Log]
GO

CREATE TABLE [Log](
	LogId INT IDENTITY(1,1) PRIMARY KEY,
	LogName NVARCHAR(50) NOT NULL,
	LogType VARCHAR(15) NOT NULL,
	[Status] VARCHAR(15) NOT NULL,
	[Description] NVARCHAR(MAX),
	CreateDate DATETIME,
	ModifyDate DATETIME,
	IsDeleted BIT
)
GO

IF OBJECT_ID('[UserLog]','U') IS NOT NULL DROP TABLE [UserLog]
GO

CREATE TABLE UserLog(
	UserId INT NOT NULL,
	LogId INT NOT NULL,
	CreateDate DATETIME,
	ModifyDate DATETIME
	PRIMARY KEY(UserId,LogId),
	FOREIGN KEY(UserId) REFERENCES [User](UserId),
	FOREIGN KEY(LogId) REFERENCES [Log](LogId),
)
GO

CREATE TRIGGER tr_AddUser
ON [User]
FOR INSERT
AS
	BEGIN
		UPDATE [User]
		SET CreateDate = GETDATE(), ModifyDate = GETDATE(), IsDeleted = 0, IsLoggedIn = 0
		FROM [User] AS U INNER JOIN inserted AS I
		ON U.UserId = I.UserId
	END
GO

CREATE TRIGGER tr_UpdateUser
ON [User]
FOR UPDATE
AS
	BEGIN
		UPDATE [User]
		SET ModifyDate = GETDATE() 
		FROM [User] AS U INNER JOIN inserted AS I
		ON U.UserId = I.UserId
	END
GO

CREATE TRIGGER tr_AddRole
ON [Role]
FOR INSERT
AS
	BEGIN
		UPDATE [Role]
		SET CreateDate = GETDATE(), ModifyDate = GETDATE(), IsDeleted = 0
		FROM [Role] AS R INNER JOIN inserted AS I 
		ON R.RoleId = I.RoleId
	END
GO

CREATE TRIGGER tr_UpdateRole
ON [Role]
FOR UPDATE
AS
	BEGIN
		UPDATE [Role]
		SET ModifyDate = GETDATE() 
		FROM [Role] AS R INNER JOIN inserted AS I 
		ON R.RoleId = I.RoleId
	END
GO

CREATE TRIGGER tr_AddLog
ON [Log]
FOR INSERT
AS
	BEGIN
		UPDATE [Log]
		SET CreateDate = GETDATE(), ModifyDate = GETDATE(), IsDeleted = 0
		FROM [Log] AS L INNER JOIN inserted AS I 
		ON L.LogId = I.LogId
	END
GO


CREATE TRIGGER tr_UpdateLog
ON [Log]
FOR UPDATE
AS
	BEGIN
		UPDATE [Log]
		SET ModifyDate = GETDATE() 
		FROM [Log] AS L INNER JOIN inserted AS I 
		ON L.LogId = I.LogId
	END
GO

CREATE TRIGGER tr_AddUserLog
ON UserLog
FOR INSERT
AS
	BEGIN
		UPDATE UserLog
		SET CreateDate = GETDATE(), ModifyDate = GETDATE()
		FROM UserLog AS UL INNER JOIN inserted AS I 
		ON UL.LogId = I.LogId and UL.UserId = I.UserId
	END
GO

--1:View 2:Create 4:Modify
INSERT INTO [Role] (RoleName,[Permissions])
VALUES ('Admin',7),
		('User',3),
		('PM', 5),
		('Guest',1)
GO

--1:Male | 0: Female
INSERT INTO [User] (Username,[Password],FullName,Birthdate,Gender,Phone,[Address],RoleId)
VALUES ('phithanh1230','12345678',N'Phạm Trung Phi Thành','1/4/1996',1,'0912572303',N'Ninh Thuận',2),
		('tuanhoang','12345678',N'Phạm Hoàng Tuấn','10/14/1996',0,'0234544732',N'Quận 7',1),
		('tienloile','12345678',N'Lê Tiến Lợi','1/1/1996',0,'0873220388',N'Quận 2',3),
		('minhtri12','12345678',N'Hứa Minh Trí','11/4/1996',1,'0912572303',N'TP.HCM',2),
		('dangvu','12345678',N'Nguyễn Đăng Vũ','10/14/1992',0,'0234544732',N'Quận Phú Nhuận',1),
		('thuphuong','12345678',N'Nguyễn Thị Thu Phượng','1/1/1996',0,'0873220388',N'Quận 9',3)

GO

INSERT INTO [Log] (LogName,LogType,[Status],[Description])
VALUES ('Do assignment BTNB','Auto Log','Open',''),
		('Display wrong','Auto Log','Open',''),
		('Find a Bug in #3-project, cant input to username','Auto Log','Open','Please fix it as fast as you can !')
GO

INSERT INTO [UserLog] (UserId,LogId)
VALUES (2,1),(2,2),(3,3),(3,1)

INSERT INTO [UserLog] (UserId,LogId)
VALUES (4,1),(5,2),(4,3)

GO

--Check exists store procedures XP_GetLogDetails
IF EXISTS(SELECT name FROM sys.procedures 
			WHERE Name = 'XP_GetLogDetails') 
			DROP PROC XP_GetLogDetails
GO

--Get details logs information.
CREATE PROC XP_GetLogDetails
	@lid int
AS
	SELECT LogName, LogType, CreateDate, ModifyDate ModifyDateLog, [Status], [Description]
	FROM [Log]
	WHERE LogId = @lid and IsDeleted = 0
GO

--Check exists store procedures XP_GetUserLogs
IF EXISTS(SELECT name FROM sys.procedures 
			WHERE Name = 'XP_GetUserLogs') 
			DROP PROC XP_GetUserLogs
GO

--Get user logs information.
CREATE PROC XP_GetUserLogs
	@lid int
AS
	SELECT U.FullName, U.UserName, UL.ModifyDate ModifyDateUserLog
	FROM UserLog UL
	INNER JOIN [User] U on U.UserId = UL.UserId
	WHERE UL.LogId = @lid and U.IsDeleted = 0
GO

--Check exists store procedures XP_GetAllLogs
IF EXISTS(SELECT name FROM sys.procedures 
			WHERE Name = 'XP_GetAllLogs') 
			DROP PROC XP_GetAllLogs
GO

--Get all logs information.
CREATE PROC XP_GetAllLogs
AS
	SELECT LogId,LogName,LogType,CreateDate,ModifyDate, [Status] 
	FROM [Log] L
	WHERE L.IsDeleted=0
GO

--Check exists store procedures XP_GetAllUsers
IF EXISTS(SELECT name FROM sys.procedures 
			WHERE Name = 'XP_GetAllUsers') 
			DROP PROC XP_GetAllUsers
GO

--Get all users information.
CREATE PROC XP_GetAllUsers
AS
	SELECT U.UserId,U.UserName,U.FullName,U.Birthdate,U.Gender,U.Phone,U.[Address],R.RoleName 
	FROM [User] U
	LEFT JOIN [Role] R ON R.RoleId = U.RoleId
	WHERE U.IsDeleted=0
GO

--Check exists store procedures XP_GetUserDetails
IF EXISTS(SELECT name FROM sys.procedures 
			WHERE Name = 'XP_GetUserDetails') 
			DROP PROC XP_GetUserDetails
GO

--Get all users information.
CREATE PROC XP_GetUserDetails
	@uid int
AS
	SELECT U.UserId,U.UserName,U.FullName,U.Birthdate,U.Gender,U.Phone,
	U.[Address],R.RoleName,U.CreateDate CreateDateUser,U.ModifyDate ModifyDateUser, L.LogName, L.LogId,
	L.LogType, L.CreateDate CreateDateLog, L.ModifyDate ModifyDateLog
	FROM [User] U
	LEFT JOIN [Role] R ON R.RoleId = U.RoleId
	LEFT JOIN [UserLog] UL ON UL.UserId = U.UserId
	LEFT JOIN [Log] L ON L.LogId = UL.LogId
	WHERE U.UserId=@uid and U.IsDeleted=0 and R.IsDeleted=0 
GO

--Check exists store procedures XP_GetAllRoles
IF EXISTS(SELECT name FROM sys.procedures 
			WHERE Name = 'XP_GetAllRoles') 
			DROP PROC XP_GetAllRoles
GO

--Get all roles logs information.
CREATE PROC XP_GetAllRoles
AS
	SELECT RoleId,RoleName,[Permissions],CreateDate,ModifyDate
	FROM [Role]
	WHERE IsDeleted=0
GO

--Check exists store procedures XP_GetRoleDetails
IF EXISTS(SELECT name FROM sys.procedures 
			WHERE Name = 'XP_GetRoleDetails') 
			DROP PROC XP_GetRoleDetails
GO

--Get detail roles logs information.
CREATE PROC XP_GetRoleDetails
	@rid int
AS
	SELECT R.RoleId,R.RoleName,R.[Permissions],R.CreateDate,R.ModifyDate, U.FullName,U.UserName
	FROM [Role] R
	LEFT JOIN [User] U ON U.RoleId = R.RoleId
	WHERE R.IsDeleted=0 and R.RoleId = @rid
GO

--Check exists store procedures XP_DeleteUser
IF EXISTS(SELECT name FROM sys.procedures 
			WHERE Name = 'XP_DeleteUser') 
			DROP PROC XP_DeleteUser
GO

--Deleted user information.
CREATE PROC XP_DeleteUser
	@UserID int
AS
	IF EXISTS (SELECT 1 FROM UserLog WHERE UserId = @UserID)
	BEGIN
		UPDATE [User] SET IsDeleted	= 1 WHERE UserId = @UserID
	END
	ELSE
	BEGIN
		DELETE FROM [User] WHERE UserId = @UserID
	END
GO

--Check exists store procedures XP_DeleteRole
IF EXISTS(SELECT name FROM sys.procedures 
			WHERE Name = 'XP_DeleteRole') 
			DROP PROC XP_DeleteRole
GO

--Deleted role information.
CREATE PROC XP_DeleteRole
	@RoleID int
AS
	IF EXISTS (SELECT 1 FROM [User] WHERE RoleId = @RoleID)
	BEGIN
		UPDATE [Role] SET IsDeleted	= 1 WHERE RoleId = @RoleID
	END
	ELSE
	BEGIN
		DELETE FROM [Role] WHERE RoleId = @RoleID
	END
GO

--Check exists store procedures XP_DeleteLog
IF EXISTS(SELECT name FROM sys.procedures 
			WHERE Name = 'XP_DeleteLog') 
			DROP PROC XP_DeleteLog
GO

--Deleted log  information.
CREATE PROC XP_DeleteLog
	@LogID int
AS
	IF EXISTS (SELECT 1 FROM UserLog WHERE LogId = @LogID)
	BEGIN
		UPDATE [Log] SET IsDeleted	= 1 WHERE LogId = @LogID
	END
	ELSE
	BEGIN
		DELETE FROM [Log] WHERE LogId = @LogID
	END
GO

--Check exists store procedures XP_CountTotal
IF EXISTS(SELECT name FROM sys.procedures 
			WHERE Name = 'XP_CountTotal') 
			DROP PROC XP_CountTotal
GO

--Get count users, logs, roles and status open, processing, done information.
CREATE PROC XP_CountTotal
AS
	SELECT(
			SELECT COUNT(U.UserId)  
			FROM [User] U
			WHERE U.IsDeleted=0 )AS TotalUsers,
			(SELECT COUNT(L.LogId)
			FROM[Log] L
			WHERE L.IsDeleted=0) AS TotalLogs,
			(SELECT COUNT(R.RoleId)
			FROM [Role] R
			WHERE R.IsDeleted=0) AS TotalRoles,
			(SELECT COUNT(L.Status)
			FROM[Log] L
			WHERE L.IsDeleted=0 AND L.Status='Open') AS TotalLogsOpen,
			(SELECT COUNT(L.Status)
			FROM[Log] L
			WHERE L.IsDeleted=0 AND L.Status='Processing') AS TotalLogsProcessing,
			(SELECT COUNT(L.Status)
			FROM[Log] L
			WHERE L.IsDeleted=0 AND L.Status='Done') AS TotalLogsDone	
						
GO

--Check exists store procedures XP_CountDay
IF EXISTS(SELECT name FROM sys.procedures 
			WHERE Name = 'XP_CountDay') 
			DROP PROC XP_CountDay
GO

--Get count logs of day input information.
CREATE PROC XP_CountDay 
			@LogDay date
AS
	SELECT COUNT(L.LogId) AS CountLogsDay
	 FROM [Log] L 
	 WHERE convert(date,L.CreateDate) = @LogDay AND l.IsDeleted=0				
GO

--Check exists store procedures XP_CountWeek
IF EXISTS(SELECT name FROM sys.procedures 
			WHERE Name = 'XP_CountWeek') 
			DROP PROC XP_CountWeek
GO

--Get count logs of day input information.
CREATE PROC XP_CountWeek 			
AS
	DECLARE @StartTime DATETIME = GETDATE();
	DECLARE @step_value int =0;
	DECLARE @count int=0
	DECLARE @WeekLogs table(dayweek date, totallogs int)
	-- Run loop table [Log] count logs in 5 day insert table
	WHILE @step_value < 5
	BEGIN
		SET @count = (
		SELECT COUNT(L.LogId)
		FROM [Log] L 
		WHERE convert(date,L.CreateDate) = convert(date,DATEADD(DAY, -@step_value ,@StartTime)) AND l.IsDeleted=0)

		INSERT @WeekLogs (dayweek, totallogs) VALUES (convert(date,DATEADD(DAY, -@step_value ,@StartTime)), @count)

		SET @step_value=@step_value+1;
	 END;

	SELECT dayweek, totallogs FROM @WeekLogs
GO


--Check exists store procedures XP_GetLogOverview
IF EXISTS(SELECT name FROM sys.procedures 
			WHERE Name = 'XP_GetLogOverview') 
			DROP PROC XP_GetLogOverview
GO

--Get Logs Overview--
CREATE PROC XP_GetLogOverview
AS
	SELECT TOP 10 L.LogName,L.CreateDate,isnull(UL.ModifyDate,L.CreateDate) ModifyDate,isnull(U.FullName,'AUTO') FullName, L.[Status]
	FROM [Log] L
	LEFT JOIN [UserLog] UL ON UL.LogId = L.LogId
	LEFT JOIN [User] U ON U.UserId = UL.UserId
	ORDER BY UL.ModifyDate desc
GO