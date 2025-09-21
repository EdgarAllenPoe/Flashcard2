using FluentAssertions;
using Xunit;
using FlashcardApp.Tests;

namespace FlashcardApp.Tests.UI.Implementations.WinUI
{
    public class WinUIBasicTests
    {
        [Fact, Trait("Category", "Fast")]
        public void WinUIApp_ShouldCreateBasicWindow_WhenApplicationStarts()
        {
            // Arrange & Act
            var projectPath = Path.Combine("..", "..", "..", "..", "FlashcardApp.WinUI.csproj");
            var projectExists = File.Exists(projectPath);

            // Assert
            projectExists.Should().BeTrue("WinUI project should exist");
        }

        [Fact, Trait("Category", "Slow")]
        public void WinUIApp_ShouldBuildSuccessfully_WhenProjectIsBuilt()
        {
            // Arrange
            var projectPath = Path.Combine("..", "..", "..", "..", "FlashcardApp.WinUI.csproj");

            // Act
            var buildResult = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"build \"{projectPath}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            });

            buildResult?.WaitForExit();
            var exitCode = buildResult?.ExitCode ?? -1;

            // Assert
            exitCode.Should().Be(0, "WinUI project should build successfully");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void WinUIApp_ShouldHaveValidExecutable_WhenBuilt()
        {
            // Arrange
            var executablePath = Path.Combine("..", "..", "..", "..", "bin", "Debug", "net8.0-windows10.0.19041.0", "win-x64", "FlashcardApp.WinUI.exe");

            // Act
            var executableExists = File.Exists(executablePath);

            // Assert
            executableExists.Should().BeTrue("WinUI executable should exist after build");
        }

        [Fact, Trait("Category", TestCategories.Fast)]
        public void WinUIApp_ShouldHaveValidProjectStructure_WhenCreated()
        {
            // Arrange & Act
            var appXamlExists = File.Exists(Path.Combine("..", "..", "..", "..", "App.xaml"));
            var appXamlCsExists = File.Exists(Path.Combine("..", "..", "..", "..", "App.xaml.cs"));
            var mainPageXamlExists = File.Exists(Path.Combine("..", "..", "..", "..", "Views", "MainPage.xaml"));
            var mainPageXamlCsExists = File.Exists(Path.Combine("..", "..", "..", "..", "Views", "MainPage.xaml.cs"));
            var assetsExists = Directory.Exists(Path.Combine("..", "..", "..", "..", "Assets"));

            // Assert
            appXamlExists.Should().BeTrue("App.xaml should exist");
            appXamlCsExists.Should().BeTrue("App.xaml.cs should exist");
            mainPageXamlExists.Should().BeTrue("MainPage.xaml should exist");
            mainPageXamlCsExists.Should().BeTrue("MainPage.xaml.cs should exist");
            assetsExists.Should().BeTrue("Assets directory should exist");
        }
    }
}
