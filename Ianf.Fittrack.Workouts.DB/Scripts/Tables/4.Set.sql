USE Fittrack;
GO
IF OBJECT_ID(N'dbo.Set', N'U') IS NULL BEGIN  

	CREATE TABLE [dbo].[Set](
		[Id] [INT] IDENTITY(1,1) NOT NULL,
		[ExerciseId] [INT] NOT NULL,
		[Reps] [INT] NOT NULL,
		[Weight] [DECIMAL] NOT NULL,
		[Order] [INT] NULL,
		CONSTRAINT [PK_SetId] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Exercise_Id] FOREIGN KEY ([ExerciseId]) REFERENCES [dbo].[Exercise]([Id])
	) WITH (DATA_COMPRESSION = PAGE)

END;
GO
