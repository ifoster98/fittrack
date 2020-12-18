
///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Test");
var configuration = Argument("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
.Does(() => {
   CleanDirectories($"./Ianf.Fittrack.Workouts/bin");
   CleanDirectories($"./Ianf.Fittrack.Workouts/obj");
   CleanDirectories($"./Ianf.Fittrack.Workouts/**/bin");
   CleanDirectories($"./Ianf.Fittrack.Workouts/**/obj");
   CleanDirectories($"./Ianf.Fittrack.Workouts.EF/bin");
   CleanDirectories($"./Ianf.Fittrack.Workouts.EF/obj");
   CleanDirectories($"./Ianf.Fittrack.Workouts.EF/**/bin");
   CleanDirectories($"./Ianf.Fittrack.Workouts.EF/**/obj");
   CleanDirectories($"./Ianf.Fittrack.Workouts.EF.Tests/bin");
   CleanDirectories($"./Ianf.Fittrack.Workouts.EF.Tests/obj");
   CleanDirectories($"./Ianf.Fittrack.Workouts.EF.Tests/**/bin");
   CleanDirectories($"./Ianf.Fittrack.Workouts.EF.Tests/**/obj");
   CleanDirectories($"./Ianf.Fittrack.Workouts.UnitTest/bin");
   CleanDirectories($"./Ianf.Fittrack.Workouts.UnitTest/obj");
   CleanDirectories($"./Ianf.Fittrack.Workouts.UnitTest/**/bin");
   CleanDirectories($"./Ianf.Fittrack.Workouts.UnitTest/**/obj");
});

Task("Build")
.IsDependentOn("Clean")
.Does(() => {
   DotNetCoreBuild("./fittrack.sln", new DotNetCoreBuildSettings
   {
      Configuration = configuration,
   });
});

Task("Test")
.IsDependentOn("Build")
.Does(() => {
    DotNetCoreTest("./fittrack.sln", new DotNetCoreTestSettings
    {
       Configuration = configuration,
       NoBuild = true
    });
});

RunTarget(target);
