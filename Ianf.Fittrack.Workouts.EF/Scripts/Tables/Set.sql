IF OBJECT_ID(N'fittrack.Set', N'U') IS NULL BEGIN  

	CREATE TABLE [fittrack].[Set](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[ExerciseId] [int] NOT NULL,
		[Reps] [int] NOT NULL,
		[Weight] [decimal] NOT NULL,
		[Order] [int] NULL,
		CONSTRAINT [PK_SetId] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Exercise_Id] FOREIGN KEY ([ExerciseId]) REFERENCES [fittrack].[Exercise]([Id])
	) WITH (DATA_COMPRESSION = PAGE)

END;
