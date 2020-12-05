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
   CleanDirectory($"./Ianf.Fittrack.Workouts.Domain/bin");
   CleanDirectory($"./Ianf.Fittrack.Workouts.Domain/obj");
   CleanDirectory($"./Ianf.Fittrack.Workouts.UnitTest.Domain/bin");
   CleanDirectory($"./Ianf.Fittrack.Workouts.UnitTest.Domain/obj");
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