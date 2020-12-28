USE Fittrack;
GO
IF OBJECT_ID(N'dbo.Exercise', N'U') IS NULL BEGIN  

	CREATE TABLE [dbo].[Exercise](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[PlannedWorkoutId] [INT] NULL,
		[ActualWorkoutId] [INT] NULL,
		[ExerciseType] [TINYINT] NOT NULL,
		[Order] [INT] NOT NULL,
		CONSTRAINT [PK_ExerciseId] PRIMARY KEY ([Id])
	) WITH (DATA_COMPRESSION = PAGE)

END;
GO

ALTER TABLE [dbo].[Exercise] WITH NOCHECK ADD 
	CONSTRAINT [FK_Planned_Workout_Id] FOREIGN KEY ([PlannedWorkoutId]) REFERENCES [dbo].[PlannedWorkout]([Id])
GO

--ALTER TABLE [dbo].[Exercise] WITH NOCHECK ADD 
--	CONSTRAINT [FK_Actual_Workout_Id] FOREIGN KEY ([ActualWorkoutId]) REFERENCES [dbo].[ActualWorkout]([Id])
--GO
