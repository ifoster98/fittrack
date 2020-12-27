#addin nuget:?package=Cake.Docker&version=0.10.0
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
});

///////////////////////////////////////////////////////////////////////////////
// RUN WEB SERVER
///////////////////////////////////////////////////////////////////////////////

Task("Run-WebServer")
.IsDependentOn("Test")
.Does(() => {
   DotNetCoreRun("Ianf.Fittrack.Webapi/Ianf.Fittrack.Webapi.csproj");
});

///////////////////////////////////////////////////////////////////////////////
// SET UP DB SERVER
///////////////////////////////////////////////////////////////////////////////

Task("Docker-Build")
.IsDependentOn("Publish")
.Does(() => {
   StartProcess("docker", new ProcessSettings {
      Arguments = new ProcessArgumentBuilder()
         .Append("build")
         .Append("-t")
         .Append(sqlDocker)
         .Append(dockerDirectory)
      }
   );
});

Task("Docker-Run")
.IsDependentOn("Docker-Build")
.Does(() => {
   StartProcess("docker", new ProcessSettings {
      Arguments = new ProcessArgumentBuilder()
         .Append("run")
         .Append("-e \"ACCEPT_EULA=Y\"")
         .Append("-e \"SA_PASSWORD=31Freeble$\"")
         .Append("-p 1433:1433")
         .Append($"--name {sqlDocker}")
         .Append($"-h {sqlDocker}")
         .Append("-d")
         .Append("mcr.microsoft.com/mssql/server:2019-latest")
      }
   );
});

Task("Create-Schema")
.IsDependentOn("Docker-Run")
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
// TEAR DOWN DATABASE
///////////////////////////////////////////////////////////////////////////////

Task("Docker-Stop")
.Does(() => {
   StartProcess("docker", new ProcessSettings {
      Arguments = new ProcessArgumentBuilder()
         .Append("container")
         .Append("stop")
         .Append(sqlDocker)
      }
   );
});

Task("Docker-Remove")
.IsDependentOn("Docker-Stop")
.Does(() => {
   StartProcess("docker", new ProcessSettings {
      Arguments = new ProcessArgumentBuilder()
         .Append("container")
         .Append("rm")
         .Append(sqlDocker)
      }
   );
});

Task("DbTests")
.IsDependentOn("Docker-Remove")
.IsDependentOn("Repository-Tests")
.Does(() => {

});

RunTarget(target);