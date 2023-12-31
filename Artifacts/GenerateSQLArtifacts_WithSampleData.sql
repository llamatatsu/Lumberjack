USE [master]
GO
/****** Object:  Database [LumberjackDB]    Script Date: 7/13/2023 10:48:50 AM ******/
CREATE DATABASE [LumberjackDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'LumberjackDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER01\MSSQL\DATA\LumberjackDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'LumberjackDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER01\MSSQL\DATA\LumberjackDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [LumberjackDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [LumberjackDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [LumberjackDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [LumberjackDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [LumberjackDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [LumberjackDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [LumberjackDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [LumberjackDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [LumberjackDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [LumberjackDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [LumberjackDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [LumberjackDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [LumberjackDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [LumberjackDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [LumberjackDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [LumberjackDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [LumberjackDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [LumberjackDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [LumberjackDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [LumberjackDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [LumberjackDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [LumberjackDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [LumberjackDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [LumberjackDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [LumberjackDB] SET RECOVERY FULL 
GO
ALTER DATABASE [LumberjackDB] SET  MULTI_USER 
GO
ALTER DATABASE [LumberjackDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [LumberjackDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [LumberjackDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [LumberjackDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [LumberjackDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [LumberjackDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'LumberjackDB', N'ON'
GO
ALTER DATABASE [LumberjackDB] SET QUERY_STORE = OFF
GO
USE [LumberjackDB]
GO
/****** Object:  Table [dbo].[Application]    Script Date: 7/13/2023 10:48:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Application](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](2000) NOT NULL,
	[ReferenceId] [varchar](20) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [varchar](100) NOT NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[File]    Script Date: 7/13/2023 10:48:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[File](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MessageId] [int] NOT NULL,
	[FileName] [varchar](100) NOT NULL,
	[InternalFilePath] [varchar](max) NOT NULL,
	[InternalFileName] [varchar](max) NOT NULL,
	[FileSize] [varchar](20) NOT NULL,
	[IsValid] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [varchar](100) NOT NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Message]    Script Date: 7/13/2023 10:48:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Message](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[MessageType] [varchar](10) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [varchar](100) NOT NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Segment]    Script Date: 7/13/2023 10:48:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Segment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MessageId] [int] NOT NULL,
	[Level] [varchar](20) NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
	[Text] [varchar](max) NOT NULL,
	[Additional] [varchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [varchar](100) NOT NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 7/13/2023 10:48:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [varchar](100) NOT NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserApplicationMap]    Script Date: 7/13/2023 10:48:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserApplicationMap](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[EncodedToken] [varchar](2000) NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[ExpiryDate] [datetime] NOT NULL,
	[Active] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [varchar](100) NOT NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Application] ON 

INSERT [dbo].[Application] ([Id], [Name], [ReferenceId], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1, N'RootFusion', N'40923SE2D32', CAST(N'2023-07-13T10:42:54.190' AS DateTime), N'Lumberjack.API', NULL, NULL)
INSERT [dbo].[Application] ([Id], [Name], [ReferenceId], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2, N'Sawmill', N'321213E2D32', CAST(N'2023-07-13T10:42:54.190' AS DateTime), N'Lumberjack.API', NULL, NULL)
INSERT [dbo].[Application] ([Id], [Name], [ReferenceId], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (3, N'ChillAccretion', N'343GCGGFG24', CAST(N'2023-07-13T10:42:54.190' AS DateTime), N'Lumberjack.API', NULL, NULL)
INSERT [dbo].[Application] ([Id], [Name], [ReferenceId], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (4, N'SaddleNotch', N'778YYTS6967', CAST(N'2023-07-13T10:42:54.190' AS DateTime), N'Lumberjack.API', NULL, NULL)
SET IDENTITY_INSERT [dbo].[Application] OFF
GO
SET IDENTITY_INSERT [dbo].[File] ON 

INSERT [dbo].[File] ([Id], [MessageId], [FileName], [InternalFilePath], [InternalFileName], [FileSize], [IsValid], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1, 4, N'SaddleNotch.20230626.log', N'\\SVBADVC1FIL01\Backup\InvestmentsIT\LumberjackAPI\Files', N'SaddleNotch.20230626.log.20230629T102845', N'10 KB', 1, CAST(N'2023-07-13T10:42:54.210' AS DateTime), N'Lumberjack.API', NULL, NULL)
SET IDENTITY_INSERT [dbo].[File] OFF
GO
SET IDENTITY_INSERT [dbo].[Message] ON 

INSERT [dbo].[Message] ([Id], [ApplicationId], [MessageType], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1, 1, N'Text', CAST(N'2023-07-13T10:42:54.203' AS DateTime), N'Lumberjack.API', NULL, NULL)
INSERT [dbo].[Message] ([Id], [ApplicationId], [MessageType], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2, 2, N'Text', CAST(N'2023-07-13T10:42:54.203' AS DateTime), N'Lumberjack.API', NULL, NULL)
INSERT [dbo].[Message] ([Id], [ApplicationId], [MessageType], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (3, 3, N'Text', CAST(N'2023-07-13T10:42:54.203' AS DateTime), N'Lumberjack.API', NULL, NULL)
INSERT [dbo].[Message] ([Id], [ApplicationId], [MessageType], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (4, 4, N'File', CAST(N'2023-07-13T10:42:54.203' AS DateTime), N'Lumberjack.API', NULL, NULL)
SET IDENTITY_INSERT [dbo].[Message] OFF
GO
SET IDENTITY_INSERT [dbo].[Segment] ON 

INSERT [dbo].[Segment] ([Id], [MessageId], [Level], [TimeStamp], [Text], [Additional], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1, 1, N'Error', CAST(N'2023-07-13T10:42:54.210' AS DateTime), N'Hearties hail-shot yawl hornswaggle parrel boatswain pinnace Pieces of Eight coxswain reef sails', N'Marooned hempen halter avast belay yard matey schooner six pounders coxswain piracy', CAST(N'2023-07-13T10:42:54.210' AS DateTime), N'Lumberjack.API', NULL, NULL)
INSERT [dbo].[Segment] ([Id], [MessageId], [Level], [TimeStamp], [Text], [Additional], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2, 1, N'Warning', CAST(N'2023-07-13T10:42:54.210' AS DateTime), N'Black spot scourge of the seven seas coxswain quarter bucko gaff maroon Blimey capstan brig', NULL, CAST(N'2023-07-13T10:42:54.210' AS DateTime), N'Lumberjack.API', NULL, NULL)
INSERT [dbo].[Segment] ([Id], [MessageId], [Level], [TimeStamp], [Text], [Additional], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (3, 2, N'Debug', CAST(N'2023-07-13T10:42:54.210' AS DateTime), N'Parrel lugger spyglass Arr lad run a rig me run a shot across the bow rum fathom', NULL, CAST(N'2023-07-13T10:42:54.210' AS DateTime), N'Lumberjack.API', NULL, NULL)
INSERT [dbo].[Segment] ([Id], [MessageId], [Level], [TimeStamp], [Text], [Additional], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (4, 2, N'Info', CAST(N'2023-07-13T10:42:54.210' AS DateTime), N'Brethren of the Coast log run a rig pirate skysail weigh anchor careen maroon Cat o''nine tails barque', N'Jib execution dock Admiral of the Black yo-ho-ho boom belaying pin lanyard Yellow Jack ahoy Blimey', CAST(N'2023-07-13T10:42:54.210' AS DateTime), N'Lumberjack.API', NULL, NULL)
INSERT [dbo].[Segment] ([Id], [MessageId], [Level], [TimeStamp], [Text], [Additional], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (5, 3, N'Fatal', CAST(N'2023-07-13T10:42:54.210' AS DateTime), N'Parrel lugger spyglass Arr lad run a rig me run a shot across the bow rum fathom', NULL, CAST(N'2023-07-13T10:42:54.210' AS DateTime), N'Lumberjack.API', NULL, NULL)
INSERT [dbo].[Segment] ([Id], [MessageId], [Level], [TimeStamp], [Text], [Additional], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (6, 3, N'Info', CAST(N'2023-07-13T10:42:54.210' AS DateTime), N'Jack Sink me driver reef tackle Jolly Roger maroon draught boom heave to', N'Carouser red ensign long boat man-of-war gunwalls quarter holystone scallywag barque coffer', CAST(N'2023-07-13T10:42:54.210' AS DateTime), N'Lumberjack.API', NULL, NULL)
SET IDENTITY_INSERT [dbo].[Segment] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([Id], [UserName], [FirstName], [LastName], [Email], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1, N'jdoe', N'John', N'Doe', N'john.doe@fglife.com', CAST(N'2023-07-13T10:42:54.193' AS DateTime), N'Lumberjack.API', NULL, NULL)
INSERT [dbo].[User] ([Id], [UserName], [FirstName], [LastName], [Email], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2, N'fubar', N'Fuljensio', N'Barr', N'fuljensio.barr@fglife.com', CAST(N'2023-07-13T10:42:54.193' AS DateTime), N'Lumberjack.API', NULL, NULL)
INSERT [dbo].[User] ([Id], [UserName], [FirstName], [LastName], [Email], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (3, N'snafu', N'Sam', N'Nafu', N'sam.nafu@fglife.com', CAST(N'2023-07-13T10:42:54.193' AS DateTime), N'Lumberjack.API', NULL, NULL)
INSERT [dbo].[User] ([Id], [UserName], [FirstName], [LastName], [Email], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (4, N'figmo', N'Frank', N'Igmo', N'sam.nafu@fglife.com', CAST(N'2023-07-13T10:42:54.193' AS DateTime), N'Lumberjack.API', NULL, NULL)
SET IDENTITY_INSERT [dbo].[User] OFF
GO
SET IDENTITY_INSERT [dbo].[UserApplicationMap] ON 

INSERT [dbo].[UserApplicationMap] ([Id], [UserId], [ApplicationId], [EncodedToken], [CreationDate], [ExpiryDate], [Active], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (1, 1, 1, N'ABC', CAST(N'2023-07-13T10:42:54.200' AS DateTime), CAST(N'2023-07-13T10:42:54.200' AS DateTime), 1, CAST(N'2023-07-13T10:42:54.200' AS DateTime), N'Lumberjack.API', NULL, NULL)
INSERT [dbo].[UserApplicationMap] ([Id], [UserId], [ApplicationId], [EncodedToken], [CreationDate], [ExpiryDate], [Active], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2, 1, 2, N'ABC', CAST(N'2023-07-13T10:42:54.200' AS DateTime), CAST(N'2023-07-13T10:42:54.200' AS DateTime), 1, CAST(N'2023-07-13T10:42:54.200' AS DateTime), N'Lumberjack.API', NULL, NULL)
INSERT [dbo].[UserApplicationMap] ([Id], [UserId], [ApplicationId], [EncodedToken], [CreationDate], [ExpiryDate], [Active], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (3, 1, 3, N'ABC', CAST(N'2023-07-13T10:42:54.200' AS DateTime), CAST(N'2023-07-13T10:42:54.200' AS DateTime), 1, CAST(N'2023-07-13T10:42:54.200' AS DateTime), N'Lumberjack.API', NULL, NULL)
INSERT [dbo].[UserApplicationMap] ([Id], [UserId], [ApplicationId], [EncodedToken], [CreationDate], [ExpiryDate], [Active], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (4, 1, 4, N'ABC', CAST(N'2023-07-13T10:42:54.200' AS DateTime), CAST(N'2023-07-13T10:42:54.200' AS DateTime), 1, CAST(N'2023-07-13T10:42:54.200' AS DateTime), N'Lumberjack.API', NULL, NULL)
INSERT [dbo].[UserApplicationMap] ([Id], [UserId], [ApplicationId], [EncodedToken], [CreationDate], [ExpiryDate], [Active], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (5, 2, 1, N'ABC', CAST(N'2023-07-13T10:42:54.200' AS DateTime), CAST(N'2023-07-13T10:42:54.200' AS DateTime), 1, CAST(N'2023-07-13T10:42:54.200' AS DateTime), N'Lumberjack.API', NULL, NULL)
INSERT [dbo].[UserApplicationMap] ([Id], [UserId], [ApplicationId], [EncodedToken], [CreationDate], [ExpiryDate], [Active], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (6, 2, 3, N'ABC', CAST(N'2023-07-13T10:42:54.200' AS DateTime), CAST(N'2023-07-13T10:42:54.200' AS DateTime), 1, CAST(N'2023-07-13T10:42:54.200' AS DateTime), N'Lumberjack.API', NULL, NULL)
INSERT [dbo].[UserApplicationMap] ([Id], [UserId], [ApplicationId], [EncodedToken], [CreationDate], [ExpiryDate], [Active], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (7, 3, 1, N'ABC', CAST(N'2023-07-13T10:42:54.200' AS DateTime), CAST(N'2023-07-13T10:42:54.200' AS DateTime), 1, CAST(N'2023-07-13T10:42:54.200' AS DateTime), N'Lumberjack.API', NULL, NULL)
INSERT [dbo].[UserApplicationMap] ([Id], [UserId], [ApplicationId], [EncodedToken], [CreationDate], [ExpiryDate], [Active], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (8, 3, 2, N'ABC', CAST(N'2023-07-13T10:42:54.200' AS DateTime), CAST(N'2023-07-13T10:42:54.200' AS DateTime), 1, CAST(N'2023-07-13T10:42:54.200' AS DateTime), N'Lumberjack.API', NULL, NULL)
INSERT [dbo].[UserApplicationMap] ([Id], [UserId], [ApplicationId], [EncodedToken], [CreationDate], [ExpiryDate], [Active], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (9, 3, 4, N'ABC', CAST(N'2023-07-13T10:42:54.200' AS DateTime), CAST(N'2023-07-13T10:42:54.200' AS DateTime), 1, CAST(N'2023-07-13T10:42:54.200' AS DateTime), N'Lumberjack.API', NULL, NULL)
SET IDENTITY_INSERT [dbo].[UserApplicationMap] OFF
GO
ALTER TABLE [dbo].[File]  WITH CHECK ADD  CONSTRAINT [FK_File_MessageId] FOREIGN KEY([MessageId])
REFERENCES [dbo].[Message] ([Id])
GO
ALTER TABLE [dbo].[File] CHECK CONSTRAINT [FK_File_MessageId]
GO
ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_ApplicationId] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[Application] ([Id])
GO
ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_ApplicationId]
GO
ALTER TABLE [dbo].[Segment]  WITH CHECK ADD  CONSTRAINT [FK_Segment_MessageId] FOREIGN KEY([MessageId])
REFERENCES [dbo].[Message] ([Id])
GO
ALTER TABLE [dbo].[Segment] CHECK CONSTRAINT [FK_Segment_MessageId]
GO
ALTER TABLE [dbo].[UserApplicationMap]  WITH CHECK ADD  CONSTRAINT [FK_UserApplicationMap_ApplicationId] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[Application] ([Id])
GO
ALTER TABLE [dbo].[UserApplicationMap] CHECK CONSTRAINT [FK_UserApplicationMap_ApplicationId]
GO
ALTER TABLE [dbo].[UserApplicationMap]  WITH CHECK ADD  CONSTRAINT [FK_UserApplicationMap_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[UserApplicationMap] CHECK CONSTRAINT [FK_UserApplicationMap_UserId]
GO
USE [master]
GO
ALTER DATABASE [LumberjackDB] SET  READ_WRITE 
GO
