IF OBJECT_ID(N'fittrack.Exercise', N'U') IS NULL BEGIN  

	CREATE TABLE [fittrack].[Exercise](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[WorkoutId] [int] NOT NULL,
		[ExerciseType] [tinyint] NOT NULL,
		[Order] [int] NULL,
		CONSTRAINT [PK_ExerciseId] PRIMARY KEY ([Id]),
	    CONSTRAINT [FK_Workout_Id] FOREIGN KEY ([WorkoutId]) REFERENCES [fittrack].[Workout]([Id])
	) WITH (DATA_COMPRESSION = PAGE)

END;
