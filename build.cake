#addin nuget:?package=Cake.Docker&version=0.11.1
#addin nuget:?package=Cake.Json&version=5.2.0
#addin nuget:?package=Newtonsoft.Json&version=11.0.2
#addin nuget:?package=Cake.Npm&version=0.17.0
using System.Runtime.CompilerServices;
using System.Threading;
///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Test");
var configuration = Argument("configuration", "Release");
var webSolution = Argument("webSolution", "./fittrack.sln");
var dbSolution = Argument("dbSolution", "./fittrackdb.sln");
var testSolution = Argument("testSolution", "./fittrackapitest.sln");
var artifactDirectory = Argument("artifactDirectory", "./artifacts/");

var server = Argument("server", "db");
var database = Argument("database", "Fittrack");
var dbUser = Argument("dbUser", "SA");
var dbPassword = Argument("dbPassword", "31Freeble$");

var dbConnectionString = $"Server={server}; Database={database}; User Id=SA; Password=31Freeble$";

///////////////////////////////////////////////////////////////////////////////
// BUILD TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
.Does(() => {
   CleanDirectory(artifactDirectory);
   DotNetCoreClean(webSolution);
   DotNetCoreClean(dbSolution);
   DotNetCoreClean(testSolution);
});

Task("Build")
.IsDependentOn("Clean")
.Does(() => {
   DotNetCoreBuild(webSolution, new DotNetCoreBuildSettings
   {
      Configuration = configuration,
   });
   DotNetCoreBuild(dbSolution, new DotNetCoreBuildSettings
   {
      Configuration = configuration,
   });
   DotNetCoreBuild(testSolution, new DotNetCoreBuildSettings
   {
      Configuration = configuration,
   });

   NpmInstall(new NpmInstallSettings {
      WorkingDirectory = "./fittrack/"
   });

   // Angular build
   StartProcess("ng", new ProcessSettings {
      WorkingDirectory = "./fittrack/",
      Arguments = new ProcessArgumentBuilder()
         .Append("build --prod")
      }
   );
});

Task("Test")
.IsDependentOn("Build")
.Does(() => {
    DotNetCoreTest("Ianf.Fittrack.Services.Tests/Ianf.Fittrack.Services.Tests.csproj", new DotNetCoreTestSettings
    {
       Configuration = configuration,
       NoBuild = true
    });
});

Task("Publish")
.IsDependentOn("Test")
.Does(() => {
   DotNetCorePublish(webSolution, new DotNetCorePublishSettings
   {
      Configuration = configuration,
      OutputDirectory = $"{artifactDirectory}/webapi/"
   });
   DotNetCorePublish(dbSolution, new DotNetCorePublishSettings
   {
      Configuration = configuration,
      OutputDirectory = $"{artifactDirectory}/db/"
   });
   CopyFile("Ianf.Fittrack.Webapi/Docker/Dockerfile", $"{artifactDirectory}/webapi/Dockerfile");
   CopyDirectory("./fittrack/dist", $"{artifactDirectory}/angular/dist");
   CopyFile("./fittrack/Dockerfile", $"{artifactDirectory}/angular/Dockerfile");
   CopyFile("./fittrack/nginx.conf", $"{artifactDirectory}/angular/nginx.conf");
});

///////////////////////////////////////////////////////////////////////////////
// SET UP LOCAL CONFIGURATION
///////////////////////////////////////////////////////////////////////////////

Task("Local-Configuration")
.IsDependentOn("Publish")
.Does(() => {
  var configFile = $"{artifactDirectory}/webapi/appsettings.json";
  dynamic config = ParseJsonFromFile(configFile);
  config.ConnectionStrings.FittrackDatabase = dbConnectionString;
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
         .Append("exec -it gtsql1 /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P \"31Freeble$\" -Q \"CREATE SCHEMA Fittrack AUTHORIZATION dbo\"")
      }
   );
});

Task("Create-Database")
.IsDependentOn("Create-Schema")
.Does(() => {
   Thread.Sleep(2000);
   StartProcess("docker", new ProcessSettings {
      Arguments = new ProcessArgumentBuilder()
         .Append("exec -it gtsql1 /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P \"31Freeble$\" -Q \"CREATE DATABASE Fittrack\"")
      }
   );
});

Task("Run-DbUp")
.IsDependentOn("Create-Database")
.Does(() => {
   StartProcess("dotnet", new ProcessSettings {
      Arguments = new ProcessArgumentBuilder()
         .Append($"{artifactDirectory}/db/Ianf.Fittrack.DB.dll")
      }
   );
});

///////////////////////////////////////////////////////////////////////////////
// TEAR DOWN INFRASTRUCTURE
///////////////////////////////////////////////////////////////////////////////

Task("DC-Down")
.Does(() => {
   if(FileExists(artifactDirectory))
      DockerComposeDown();
});

///////////////////////////////////////////////////////////////////////////////
// REBUILD ENVIRONMENT
///////////////////////////////////////////////////////////////////////////////
Task("Rebuild")
.IsDependentOn("DC-Down")
.IsDependentOn("Run-DbUp")
.Does(() => {

});

///////////////////////////////////////////////////////////////////////////////
// TEST API LEVEL
///////////////////////////////////////////////////////////////////////////////

Task("API-Tests")
.IsDependentOn("Rebuild")
.Does(() => {
    DotNetCoreTest("Ianf.Fittrack.Webapi.Tests/Ianf.Fittrack.Webapi.Tests.csproj", new DotNetCoreTestSettings
    {
       Configuration = configuration,
       NoBuild = true
    });
});

RunTarget(target);