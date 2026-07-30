CREATE TABLE [dbo].[book](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[title] [varchar](150) NOT NULL,
	[author] [varchar](100) NOT NULL,
	[price] [decimal](10,2) NOT NULL,
	[launch_date] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)
) ON [PRIMARY]
GO