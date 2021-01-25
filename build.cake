#addin nuget:?package=Cake.Docker&version=0.11.1
#addin nuget:?package=Cake.Json&version=5.2.0
#addin nuget:?package=Newtonsoft.Json&version=11.0.2
#addin nuget:?package=Cake.Npm&version=0.17.0
#addin nuget:?package=Cake.FileHelpers&version=3.3.0
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

Task("Publish-DockerCompose-Test")
.IsDependentOn("Test")
.Does(() => {
   EnsureDirectoryExists($"{artifactDirectory}/dockertest/");
   var dockerCompose = FileReadText("./docker-compose-template.yaml");
   dockerCompose = dockerCompose.Replace("ENVIRONMENT", "test");
   dockerCompose = dockerCompose.Replace("SERVICE_PORT", "8080");
   dockerCompose = dockerCompose.Replace("WEB_PORT", "8081");
   FileWriteText($"{artifactDirectory}/dockertest/docker-compose.yaml", dockerCompose);
});

Task("Publish-DockerCompose-Prod")
.IsDependentOn("Test")
.Does(() => {
   EnsureDirectoryExists($"{artifactDirectory}/dockerprod/");
   var dockerCompose = FileReadText("./docker-compose-template.yaml");
   dockerCompose = dockerCompose.Replace("ENVIRONMENT", "prod");
   dockerCompose = dockerCompose.Replace("SERVICE_PORT", "8082");
   dockerCompose = dockerCompose.Replace("WEB_PORT", "80");
   FileWriteText($"{artifactDirectory}/dockerprod/docker-compose.yaml", dockerCompose);
});

Task("Publish")
.IsDependentOn("Publish-Aspnet")
.IsDependentOn("Publish-DockerFiles")
.IsDependentOn("Publish-DockerCompose-Test")
.IsDependentOn("Publish-DockerCompose-Prod")
.Does(() => {

});

///////////////////////////////////////////////////////////////////////////////
// SET UP TEST INFRASTRUCTURE
///////////////////////////////////////////////////////////////////////////////

Task("DC-Up-Test")
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
// TEAR DOWN TEST INFRASTRUCTURE
///////////////////////////////////////////////////////////////////////////////

Task("DC-Down-Test")
.Does(() => {
   if(FileExists(artifactDirectory))
      DockerComposeDown(new DockerComposeDownSettings
      {
         Files = new string[] {$"{artifactDirectory}/dockertest/docker-compose.yaml"},
         Volumes = false
      });
});

///////////////////////////////////////////////////////////////////////////////
// SET UP PROD INFRASTRUCTURE
///////////////////////////////////////////////////////////////////////////////

Task("DC-Up-Prod")
.IsDependentOn("Publish")
.Does(() => {
   DockerComposeUp(new DockerComposeUpSettings
   {
      Files = new string[] {$"{artifactDirectory}/dockerprod/docker-compose.yaml"},
      ForceRecreate=true,
      DetachedMode=true,
      Build=true
   }); 
});

///////////////////////////////////////////////////////////////////////////////
// TEAR DOWN PROD INFRASTRUCTURE
///////////////////////////////////////////////////////////////////////////////

Task("DC-Down-Prod")
.Does(() => {
   if(FileExists(artifactDirectory))
      DockerComposeDown(new DockerComposeDownSettings
      {
         Files = new string[] {$"{artifactDirectory}/dockerprod/docker-compose.yaml"},
         Volumes = false
      });
});

///////////////////////////////////////////////////////////////////////////////
// REBUILD TEST ENVIRONMENT
///////////////////////////////////////////////////////////////////////////////
Task("Rebuild-Test")
.IsDependentOn("DC-Down-Test")
.IsDependentOn("DC-Up-Test")
.Does(() => {

});

///////////////////////////////////////////////////////////////////////////////
// REBUILD PROD ENVIRONMENT
///////////////////////////////////////////////////////////////////////////////
Task("Rebuild-Prod")
.IsDependentOn("DC-Down-Prod")
.IsDependentOn("DC-Up-Prod")
.Does(() => {

});

///////////////////////////////////////////////////////////////////////////////
// TEST API LEVEL
///////////////////////////////////////////////////////////////////////////////

Task("API-Tests")
.IsDependentOn("Rebuild-Test")
.Does(() => {
    DotNetCoreTest("Ianf.Fittrack.Webapi.Tests/Ianf.Fittrack.Webapi.Tests.csproj", new DotNetCoreTestSettings
    {
       Configuration = configuration,
       NoBuild = true
    });
});

RunTarget(target);