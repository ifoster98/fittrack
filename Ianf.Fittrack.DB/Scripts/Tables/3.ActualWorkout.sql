USE Fittrack;
GO
IF OBJECT_ID(N'dbo.ActualWorkout', N'U') IS NULL BEGIN  

	CREATE TABLE [dbo].[ActualWorkout](
		[Id] [int] IDENTITY(1,1) NOT NULL,
        [ProgramName] [VARCHAR](255) NOT NULL,
		[ProgramType] [TINYINT] NOT NULL,
		[WorkoutTime] [DATETIME] NOT NULL,
		CONSTRAINT [PK_ActualWorkoutId] PRIMARY KEY ([Id])
	) WITH (DATA_COMPRESSION = PAGE)

END;
GO
