#tool "nuget:?package=NUnit.ConsoleRunner"

var target = Argument("target", "Default");
var solution = File("./Bugsnag.Unity.sln");
var configuration = Argument("configuration", "Release");

Task("Restore-NuGet-Packages")
    .Does(() => NuGetRestore(solution));

Task("Build")
  .IsDependentOn("Restore-NuGet-Packages")
  .Does(() => {
    MSBuild(solution, settings =>
      settings
        .SetVerbosity(Verbosity.Minimal)
        .SetConfiguration(configuration));
  });

Task("Test")
  .IsDependentOn("Build")
  .Does(() => {
    var assemblies = GetFiles($"./tests/**/bin/{configuration}/**/*.Tests.dll");
    NUnit3(assemblies);
  });

Task("Default")
  .IsDependentOn("Test");

RunTarget(target);