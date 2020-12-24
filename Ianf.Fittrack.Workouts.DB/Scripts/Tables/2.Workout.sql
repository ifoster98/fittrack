USE Fittrack;
GO
IF OBJECT_ID(N'dbo.Workout', N'U') IS NULL BEGIN  

	CREATE TABLE [dbo].[Workout](
		[Id] [int] IDENTITY(1,1) NOT NULL,
        [ProgramName] [VARCHAR](255) NOT NULL,
		[WorkoutTime] [DATETIME] NOT NULL,
		CONSTRAINT [PK_WorkoutId] PRIMARY KEY ([Id])
	) WITH (DATA_COMPRESSION = PAGE)

END;
GO
