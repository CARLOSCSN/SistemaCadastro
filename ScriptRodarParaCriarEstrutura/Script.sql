/*    ==Parâmetros de Script==

    Versão do Servidor de Origem : SQL Server 2012 (11.0.2218)
    Edição do Mecanismo de Banco de Dados de Origem : Microsoft SQL Server Express Edition
    Tipo do Mecanismo de Banco de Dados de Origem : SQL Server Autônomo

    Versão do Servidor de Destino : SQL Server 2017
    Edição de Mecanismo de Banco de Dados de Destino : Microsoft SQL Server Standard Edition
    Tipo de Mecanismo de Banco de Dados de Destino : SQL Server Autônomo
*/

IF  NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'db_corelogin')
    CREATE DATABASE [db_corelogin]

IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'db_corelogin')
USE [db_corelogin]

/****** Object:  StoredProcedure [dbo].[LoginByUsernamePassword]  ******/  
DROP PROCEDURE [dbo].[LoginByUsernamePassword]  
GO  
/****** Object:  Table [dbo].[Item] ******/  
DROP TABLE [dbo].[Item]  
GO  
/****** Object:  Table [dbo].[Login] ******/  
DROP TABLE [dbo].[Login]  
GO  
/****** Object:  Table [dbo].[Login]  ******/  

GO
/****** Object:  Table [dbo].[Item]    Script Date: 04/12/2018 15:21:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item](
	[ItemID] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [nvarchar](50) NULL,
	[Tipo] [nvarchar](50) NULL,
	[Valor] [money] NOT NULL,
	[DataInclusao] [datetime2](7) NOT NULL,
	[LoginID] [int] NULL,
	[RowVersion] [timestamp] NULL,
	[LoginNome] [nvarchar](max) NULL,
 CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED 
(
	[ItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Login]    Script Date: 04/12/2018 15:21:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Login](
	[LoginID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Login] PRIMARY KEY CLUSTERED 
(
	[LoginID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_Login_LoginID] FOREIGN KEY([LoginID])
REFERENCES [dbo].[Login] ([LoginID])
GO
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK_Item_Login_LoginID]
GO
INSERT [dbo].[Login] ([username], [password]) VALUES (N'User', N'123')  
SET IDENTITY_INSERT [dbo].[Login] OFF  
/****** Object:  StoredProcedure [dbo].[LoginByUsernamePassword]    Script Date: 04/12/2018 15:21:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================  
-- Author:  <Author,Carlos Santana>  
-- Create date: <Create Date,,15-Mar-2016>  
-- =============================================  
CREATE PROCEDURE [dbo].[LoginByUsernamePassword]   
 @username varchar(50),  
 @password varchar(50)  
AS  
BEGIN  
 SELECT LoginID, username, password  
 FROM Login  
 WHERE username = @username  
 AND password = @password  
END  
  
GO
