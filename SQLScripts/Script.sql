USE [OwnWork]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Contracts]') AND type in (N'U'))
DROP TABLE [dbo].[Contracts]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CoveragePlan]') AND type in (N'U'))
DROP TABLE [dbo].[CoveragePlan]
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RateChart]') AND type in (N'U'))
DROP TABLE [dbo].[RateChart]
GO

/****** Object:  Table [dbo].[Contracts]******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Contracts](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CustomerName] [nchar](100) NOT NULL,
	[CustomerAddress] [nchar](255) NULL,
	[CustomerGender] [nchar](1) NOT NULL,
	[CustomerCountry] [nchar](10) NOT NULL,
	[CustomerDateofBirth] [date] NOT NULL,
	[SaleDate] [date] NOT NULL,
	[CoveragePlan] [nchar](10) NULL,
	[NetPrice] [int] NULL,
 CONSTRAINT [PK_Contracts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[CoveragePlan] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CoveragePlan](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CoveragePlan] [nchar](15) NOT NULL,
	[EligibilityDateFrom] [date] NOT NULL,
	[EligibilityDateTo] [date] NOT NULL,
	[EligibilityCountry] [nchar](10) NOT NULL,
	CONSTRAINT [PK_CoveragePlan] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[RateChart]  ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RateChart](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CoveragePlan] [nchar](15) NOT NULL,
	[CustomerGender] [nchar](1) NOT NULL,
	[CustomerAge] [nchar](10) NOT NULL,
	[NetPrice] [int] NOT NULL,
	CONSTRAINT [PK_RateChart] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


----------------Initial Data For Coverage Plan and Rate Chart-------------------------------

INSERT [dbo].[CoveragePlan] ([CoveragePlan], [EligibilityDateFrom], [EligibilityDateTo], [EligibilityCountry]) VALUES (N'Gold', CAST(N'2009-01-01' AS Date), CAST(N'2021-01-01' AS Date), N'USA')
GO
INSERT [dbo].[CoveragePlan] ([CoveragePlan], [EligibilityDateFrom], [EligibilityDateTo], [EligibilityCountry]) VALUES (N'Platinum', CAST(N'2005-01-01' AS Date), CAST(N'2023-01-01' AS Date), N'CAN')
GO
INSERT [dbo].[CoveragePlan] ([CoveragePlan], [EligibilityDateFrom], [EligibilityDateTo], [EligibilityCountry]) VALUES (N'Silver', CAST(N'2001-01-01' AS Date), CAST(N'2026-01-01' AS Date), N'*(any)')
GO

INSERT [dbo].[RateChart] ([CoveragePlan], [CustomerGender], [CustomerAge], [NetPrice]) VALUES (N'Gold', N'M', N'<=40', 1000)
GO
INSERT [dbo].[RateChart] ([CoveragePlan], [CustomerGender], [CustomerAge], [NetPrice]) VALUES (N'Gold', N'M', N'>40', 2000)
GO
INSERT [dbo].[RateChart] ([CoveragePlan], [CustomerGender], [CustomerAge], [NetPrice]) VALUES (N'Gold', N'F', N'<=40', 1200)
GO
INSERT [dbo].[RateChart] ([CoveragePlan], [CustomerGender], [CustomerAge], [NetPrice]) VALUES (N'Gold', N'F', N'>40', 2500)
GO
INSERT [dbo].[RateChart] ([CoveragePlan], [CustomerGender], [CustomerAge], [NetPrice]) VALUES (N'Silver', N'M', N'<=40', 1500)
GO
INSERT [dbo].[RateChart] ([CoveragePlan], [CustomerGender], [CustomerAge], [NetPrice]) VALUES (N'Silver', N'M', N'>40 ', 2600)
GO
INSERT [dbo].[RateChart] ([CoveragePlan], [CustomerGender], [CustomerAge], [NetPrice]) VALUES (N'Silver', N'F', N'<=40', 1900)
GO
INSERT [dbo].[RateChart] ([CoveragePlan], [CustomerGender], [CustomerAge], [NetPrice]) VALUES (N'Silver', N'F', N'>40', 2800)
GO
INSERT [dbo].[RateChart] ([CoveragePlan], [CustomerGender], [CustomerAge], [NetPrice]) VALUES (N'Platinum', N'M', N'<=40', 1900)
GO
INSERT [dbo].[RateChart] ([CoveragePlan], [CustomerGender], [CustomerAge], [NetPrice]) VALUES (N'Platinum', N'M', N'>40', 2900)
GO
INSERT [dbo].[RateChart] ([CoveragePlan], [CustomerGender], [CustomerAge], [NetPrice]) VALUES (N'Platinum', N'F', N'<=40', 2100)
GO
INSERT [dbo].[RateChart] ([CoveragePlan], [CustomerGender], [CustomerAge], [NetPrice]) VALUES (N'Platinum', N'F', N'>40', 3200)
GO

