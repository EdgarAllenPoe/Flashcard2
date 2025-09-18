using FluentAssertions;
using System.Reflection;
using Xunit;

namespace FlashcardApp.Tests
{
    public class IconTests
    {
        [Fact]
        public void ProjectFile_ShouldReferenceIconFile()
        {
            // Arrange - Read the project file content
            var projectContent = File.ReadAllText("../../../../FlashcardApp.csproj");

            // Act & Assert
            projectContent.Should().Contain("ApplicationIcon", "The project file should reference an ApplicationIcon");
        }

        [Fact]
        public void IconFile_ShouldExist()
        {
            // Act & Assert
            File.Exists("../../../../app.ico").Should().BeTrue("The app.ico file should exist in the project root");
        }

        [Fact]
        public void IconFile_ShouldBeValidIcoFormat()
        {
            // Arrange
            var iconPath = "../../../../app.ico";
            
            // Act
            var iconBytes = File.ReadAllBytes(iconPath);

            // Assert
            iconBytes.Should().NotBeEmpty("Icon file should not be empty");
            iconBytes.Length.Should().BeGreaterThan(0, "Icon file should have content");
            
            // Check for ICO file signature (first 4 bytes should be 0x00 0x00 0x01 0x00)
            if (iconBytes.Length >= 4)
            {
                var signature = new byte[] { 0x00, 0x00, 0x01, 0x00 };
                var actualSignature = iconBytes.Take(4).ToArray();
                actualSignature.Should().BeEquivalentTo(signature, "Icon file should have valid ICO format signature");
            }
        }

        [Fact]
        public void IconFile_ShouldHaveReasonableSize()
        {
            // Arrange
            var iconPath = "../../../../app.ico";
            
            // Act
            var iconBytes = File.ReadAllBytes(iconPath);

            // Assert
            iconBytes.Length.Should().BeGreaterThan(100, "Icon file should be reasonably sized");
            iconBytes.Length.Should().BeLessThan(10000, "Icon file should not be excessively large");
        }
    }
}
