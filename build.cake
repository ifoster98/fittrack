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
var testSolution = Argument("testSolution", "./fittrackapitest.sln");
var artifactDirectory = Argument("artifactDirectory", "./artifacts/");

///////////////////////////////////////////////////////////////////////////////
// BUILD TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
.Does(() => {
   CleanDirectory(artifactDirectory);
   DotNetCoreClean(webSolution);
   DotNetCoreClean(testSolution);
});

Task("Build")
.IsDependentOn("Clean")
.Does(() => {
   DotNetCoreBuild(webSolution, new DotNetCoreBuildSettings
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
//   StartProcess("ng", new ProcessSettings {
//      WorkingDirectory = "./fittrack/",
//      Arguments = new ProcessArgumentBuilder()
//         .Append("build --prod")
//      }
//   );
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

Task("Publish-AspNet")
.IsDependentOn("Test")
.Does(() => {
   DotNetCorePublish(webSolution, new DotNetCorePublishSettings
   {
      Configuration = configuration,
      OutputDirectory = $"{artifactDirectory}/webapi/"
   });
});

Task("Publish-DockerFiles")
.IsDependentOn("Test")
.Does(() => {
   CopyFile("Ianf.Fittrack.Webapi/Docker/Dockerfile", $"{artifactDirectory}/webapi/Dockerfile");
   CopyDirectory("./fittrack/dist", $"{artifactDirectory}/angular/dist");
   CopyFile("./fittrack/Dockerfile", $"{artifactDirectory}/angular/Dockerfile");
   CopyFile("./fittrack/nginx.conf", $"{artifactDirectory}/angular/nginx.conf");
});

Task("Publish-DockerCompose")
.IsDependentOn("Test")
.Does(() => {
   EnsureDirectoryExists($"{artifactDirectory}/dockertest/");
   CopyFile("./docker-compose-template.yaml", $"{artifactDirectory}/dockertest/docker-compose.yaml");
});

Task("Publish")
.IsDependentOn("Publish-Aspnet")
.IsDependentOn("Publish-DockerFiles")
.IsDependentOn("Publish-DockerCompose")
.Does(() => {

});
///////////////////////////////////////////////////////////////////////////////
// SET UP INFRASTRUCTURE
///////////////////////////////////////////////////////////////////////////////

Task("DC-Up")
.IsDependentOn("Publish")
.Does(() => {
   DockerComposeUp(new DockerComposeUpSettings
   {
      Files = new string[] {$"{artifactDirectory}/dockertest/docker-compose.yaml"},
      ForceRecreate=true,
      DetachedMode=true,
      Build=true
   }); 
});

///////////////////////////////////////////////////////////////////////////////
// TEAR DOWN INFRASTRUCTURE
///////////////////////////////////////////////////////////////////////////////

Task("DC-Down")
.Does(() => {
   if(FileExists(artifactDirectory))
      DockerComposeDown(new DockerComposeDownSettings
      {
         Files = new string[] {$"{artifactDirectory}/dockertest/docker-compose.yaml"},
      });
});

///////////////////////////////////////////////////////////////////////////////
// REBUILD ENVIRONMENT
///////////////////////////////////////////////////////////////////////////////
Task("Rebuild")
.IsDependentOn("DC-Down")
.IsDependentOn("DC-Up")
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