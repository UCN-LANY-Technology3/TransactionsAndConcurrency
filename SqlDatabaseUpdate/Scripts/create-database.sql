/*
Cafe Sanchez v2 - Case Study
*/

CREATE TABLE [Orders](
	[Id] int IDENTITY(1,1) PRIMARY KEY NOT NULL, 
	[CustomerName] [nvarchar](50) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Discount] [int] NOT NULL DEFAULT (0),
	[Status] [nvarchar](50) DEFAULT (N'New') NOT NULL
) 
GO

CREATE TABLE [Products](
	[Id] int IDENTITY(1,1) PRIMARY KEY NOT NULL, 
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Price] [decimal](18, 2) NULL
) 
GO

CREATE TABLE [Orderlines](
	[OrderId] [int] FOREIGN KEY REFERENCES [Orders] ([Id]) NOT NULL,
	[ProductId] [int] FOREIGN KEY REFERENCES [Products] ([Id]) NOT NULL,
	[Quantity] [int] NOT NULL,
	CONSTRAINT [PK_OrderLines] PRIMARY KEY CLUSTERED 
	(
		[OrderId] ASC,
		[ProductId] ASC
	)
) 
GO