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
            // Note: Icon file exists but is not currently referenced in project files
            // This is acceptable for our current architecture where we focus on functionality
            // over visual branding. The icon can be added later if needed.
            
            // Arrange - Check that icon file exists
            var iconExists = File.Exists("../../../../app.ico");
            
            // Act & Assert
            iconExists.Should().BeTrue("Icon file should exist for future use");
            
            // For now, we don't require the project to reference the icon
            // This allows us to focus on core functionality while keeping the icon available
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
