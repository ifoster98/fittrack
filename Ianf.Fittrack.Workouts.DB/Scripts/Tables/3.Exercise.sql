USE Fittrack;
GO
IF OBJECT_ID(N'dbo.Exercise', N'U') IS NULL BEGIN  

	CREATE TABLE [dbo].[Exercise](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[WorkoutId] [INT] NOT NULL,
		[ExerciseType] [TINYINT] NOT NULL,
		[Order] [INT] NOT NULL,
		CONSTRAINT [PK_ExerciseId] PRIMARY KEY ([Id]),
	    CONSTRAINT [FK_Workout_Id] FOREIGN KEY ([WorkoutId]) REFERENCES [dbo].[Workout]([Id])
	) WITH (DATA_COMPRESSION = PAGE)

END;
GO
