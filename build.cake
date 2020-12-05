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
   CleanDirectory($"./Ianf.Fittrack.Domain/bin");
   CleanDirectory($"./Ianf.Fittrack.Domain/obj");
   CleanDirectory($"./Ianf.Fittrack.UnitTest/bin");
   CleanDirectory($"./Ianf.Fittrack.UnitTest/obj");
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