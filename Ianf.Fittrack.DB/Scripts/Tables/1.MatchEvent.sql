USE Fittrack;
GO
IF OBJECT_ID(N'dbo.MatchEvent', N'U') IS NULL BEGIN  

	CREATE TABLE [dbo].[MatchEvent](
		[Id] [int] IDENTITY(1,1) NOT NULL,
        [UserId] [int] NOT NULL,
        [MatchId] [int] NOT NULL,
		[EventTime] [DATETIME] NOT NULL,
        [MatchEventType] [int] NOT NULL,
		CONSTRAINT [PK_MatchEventId] PRIMARY KEY ([Id])
	) WITH (DATA_COMPRESSION = PAGE)

END;
GO
