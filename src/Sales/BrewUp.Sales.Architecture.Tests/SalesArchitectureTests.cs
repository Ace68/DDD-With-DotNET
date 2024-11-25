using NetArchTest.Rules;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace BrewUp.Sales.Architecture.Tests;

[ExcludeFromCodeCoverage]
public class SalesArchitectureTests
{
    [Fact]
    public void SalesProjects_Should_Having_Namespace_StartingWith_BrewUp_Sales()
    {
        var sourceModulePath = Path.Combine(ProjectFileProvider.TryGetSolutionDirectoryInfo().FullName, "Sales");
        var subFolders = Directory.GetDirectories(sourceModulePath);

        var netVersion = Environment.Version;

        var moduleAssemblies = (from folder in subFolders
                                   let binFolder = Path.Join(folder, "bin", "Debug", $"net{netVersion.Major}.{netVersion.Minor}")
                                   let files = Directory.GetFiles(binFolder)
                                   let folderArray = folder.Split(Path.DirectorySeparatorChar)
                                   select files.FirstOrDefault(f => f.EndsWith($"{folderArray[folderArray!.Length - 1]}.dll"))
            into assemblyFilename
                                   where !assemblyFilename!.Contains("Test")
                                   select Assembly.LoadFile(assemblyFilename!)).ToList();

        var moduleTypes = Types.InAssemblies(moduleAssemblies);
        var moduleResult = moduleTypes
            .Should()
            .ResideInNamespaceStartingWith("BrewUp.Sales")
            .GetResult();

        Assert.True(moduleResult.IsSuccessful);
    }
    
    [Fact]
    public void Should_SalesArchitecture_BeCompliant()
    {
        var types = Types.InAssembly(typeof(Facade.SalesFacade).Assembly);

        var forbiddenAssemblies = new List<string>
        {
            "BrewUp.Warehouses.Facade",
            "BrewUp.Warehouses.Domain",
            "BrewUp.Warehouses.Infrastructures",
            "BrewUp.Warehouses.ReadModel",
            "BrewUp.Warehouses.SharedKernel"
        };

        var result = types
            .ShouldNot()
            .HaveDependencyOnAny(forbiddenAssemblies.ToArray())
            .GetResult()
            .IsSuccessful;

        Assert.True(result);
    }

    private static class ProjectFileProvider
    {
        public static DirectoryInfo TryGetSolutionDirectoryInfo(string? currentPath = null)
        {
            var directory = new DirectoryInfo(
                currentPath ?? Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return directory;
        }
    }
}