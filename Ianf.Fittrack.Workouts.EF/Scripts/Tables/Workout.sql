IF OBJECT_ID(N'fittrack.Workout', N'U') IS NULL BEGIN  

	CREATE TABLE [fittrack].[Workout](
		[Id] [int] IDENTITY(1,1) NOT NULL,
        [ProgramName] [varchar(255)] NOT NULL,
		[WorkoutTime] [datetimeoffset](7) NOT NULL,
		CONSTRAINT [PK_WorkoutId] PRIMARY KEY ([Id])
	) WITH (DATA_COMPRESSION = PAGE)

END;
