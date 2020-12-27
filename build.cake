#addin nuget:?package=Cake.Docker&version=0.11.1
#addin nuget:?package=Cake.Json&version=5.2.0
#addin nuget:?package=Newtonsoft.Json&version=11.0.2
using System.Runtime.CompilerServices;
using System.Threading;
///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Test");
var configuration = Argument("configuration", "Release");
var solution = Argument("solution", "./fittrack.sln");
var dbSolution = Argument("dbSolution", "./fittrackdb.sln");
var dockerDirectory = Argument("dockerDirectory", "./Ianf.Fittrack.Workouts.DB/Docker");
var homeDirectory = Argument("homeDirectory", "/Users/ianfoster/dev/fittrack/");
var sqlDocker = Argument("sqlDocker", "sql1");
var artifactDirectory = Argument("artifactDirectory", "./artifacts/");

///////////////////////////////////////////////////////////////////////////////
// BUILD TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
.Does(() => {
   CleanDirectory(artifactDirectory);
   DotNetCoreClean(solution);
   DotNetCoreClean(dbSolution);
});

Task("Build")
.IsDependentOn("Clean")
.Does(() => {
   DotNetCoreBuild(solution, new DotNetCoreBuildSettings
   {
      Configuration = configuration,
   });
   DotNetCoreBuild(dbSolution, new DotNetCoreBuildSettings
   {
      Configuration = configuration,
   });
});

Task("Test")
.IsDependentOn("Build")
.Does(() => {
    DotNetCoreTest(solution, new DotNetCoreTestSettings
    {
       Configuration = configuration,
       NoBuild = true,
       Filter = "FullyQualifiedName!~WorkoutRepositoryTests"
    });
});

Task("Publish")
.IsDependentOn("Test")
.Does(() => {
   DotNetCorePublish(solution, new DotNetCorePublishSettings
   {
      Configuration = configuration,
      OutputDirectory = $"{artifactDirectory}/webapp/"
   });
   DotNetCorePublish(dbSolution, new DotNetCorePublishSettings
   {
      Configuration = configuration,
      OutputDirectory = $"{artifactDirectory}/db/"
   });
   CopyFile("Ianf.Fittrack.Webapi/Docker/Dockerfile", $"{artifactDirectory}/Dockerfile");
});

///////////////////////////////////////////////////////////////////////////////
// SET UP LOCAL CONFIGURATION
///////////////////////////////////////////////////////////////////////////////

Task("Local-Configuration")
.IsDependentOn("Publish")
.Does(() => {
  var configFile = $"{artifactDirectory}/webapp/appsettings.json";
  dynamic config = ParseJsonFromFile(configFile);
  config.ConnectionStrings.FittrackDatabase = "Server=db; Database=Fittrack; User Id=SA; Password=31Freeble$";
  SerializeJsonToPrettyFile<JObject>(configFile, config);
});

///////////////////////////////////////////////////////////////////////////////
// SET UP INFRASTRUCTURE
///////////////////////////////////////////////////////////////////////////////

Task("DC-Up")
.IsDependentOn("Local-Configuration")
.Does(() => {
   DockerComposeUp(new DockerComposeUpSettings
   {
      ForceRecreate=true,
      DetachedMode=true,
      Build=true
   }); 
});

///////////////////////////////////////////////////////////////////////////////
// SET UP DB SERVER
///////////////////////////////////////////////////////////////////////////////

Task("Create-Schema")
.IsDependentOn("DC-Up")
.Does(() => {
   Thread.Sleep(2000);
   StartProcess("docker", new ProcessSettings {
      Arguments = new ProcessArgumentBuilder()
         .Append("exec -it sql1 /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P \"31Freeble$\" -Q \"CREATE SCHEMA Fittrack AUTHORIZATION dbo\"")
      }
   );
});

Task("Create-Database")
.IsDependentOn("Create-Schema")
.Does(() => {
   Thread.Sleep(2000);
   StartProcess("docker", new ProcessSettings {
      Arguments = new ProcessArgumentBuilder()
         .Append("exec -it sql1 /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P \"31Freeble$\" -Q \"CREATE DATABASE Fittrack\"")
      }
   );
});

Task("Run-DbUp")
.IsDependentOn("Create-Database")
.Does(() => {
   StartProcess("dotnet", new ProcessSettings {
      Arguments = new ProcessArgumentBuilder()
         .Append($"{artifactDirectory}/db/Ianf.Fittrack.Workouts.DB.dll")
      }
   );
});

///////////////////////////////////////////////////////////////////////////////
// TEST REPOSITORY CODE
///////////////////////////////////////////////////////////////////////////////

Task("Repository-Tests")
.IsDependentOn("Run-DbUp")
.Does(() => {
    DotNetCoreTest(solution, new DotNetCoreTestSettings
    {
       Configuration = configuration,
       NoBuild = true,
       Filter = "FullyQualifiedName~WorkoutRepositoryTests"
    });
});

///////////////////////////////////////////////////////////////////////////////
// TEAR DOWN INFRASTRUCTURE
///////////////////////////////////////////////////////////////////////////////

Task("DC-Down")
.IsDependentOn("DC-Up")
.Does(() => {
   DockerComposeDown();
});

RunTarget(target);