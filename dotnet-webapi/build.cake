#addin nuget:?package=Cake.Coverlet

var target = Argument("target", "Test");
var configuration = Argument("configuration", "Debug");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////


Task("Build")
    .Does(() =>
{
    DotNetBuild("WebApisln", new DotNetBuildSettings
    {
        Configuration = configuration,
    });
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
	var coverletSettings = new CoverletSettings {
        CollectCoverage = true,
        CoverletOutputFormat = CoverletOutputFormat.opencover | CoverletOutputFormat.json,
        MergeWithFile = MakeAbsolute(new DirectoryPath("./coverage.json")).FullPath,
        CoverletOutputDirectory = MakeAbsolute(new DirectoryPath(@"./coverage")).FullPath
    };
	
	Coverlet(
        "./tests/Api.IntegrationTests/bin/Debug/net7.0/Api.IntegrationTests.dll", 
        "./tests/Api.IntegrationTests/Api.IntegrationTests.csproj", 
        coverletSettings);
		
	Coverlet(
        "./tests/Api.UnitTests/bin/Debug/net7.0/Api.UnitTests.dll", 
        "./tests/Api.UnitTests/Api.UnitTests.csproj", 
        coverletSettings);
	
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);