using System;
using System.Collections.Generic;
using System.Linq;

namespace FlashcardApp.UI.Design
{
    /// <summary>
    /// Represents a color in the design system
    /// </summary>
    public class Color
    {
        public string Name { get; set; } = string.Empty;
        public string Hex { get; set; } = string.Empty;
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public double Opacity { get; set; } = 1.0;
    }

    /// <summary>
    /// Represents a complete color palette
    /// </summary>
    public class ColorPalette
    {
        public Color Primary { get; set; } = new Color();
        public Color Secondary { get; set; } = new Color();
        public Color Success { get; set; } = new Color();
        public Color Warning { get; set; } = new Color();
        public Color Error { get; set; } = new Color();
        public Color Info { get; set; } = new Color();
        public Color Neutral { get; set; } = new Color();
    }

    /// <summary>
    /// Represents typography styles
    /// </summary>
    public class TypographyStyle
    {
        public string FontFamily { get; set; } = string.Empty;
        public double FontSize { get; set; }
        public double LineHeight { get; set; }
        public string FontWeight { get; set; } = string.Empty;
        public string FontStyle { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents the complete typography scale
    /// </summary>
    public class TypographyScale
    {
        public TypographyStyle Heading1 { get; set; } = new TypographyStyle();
        public TypographyStyle Heading2 { get; set; } = new TypographyStyle();
        public TypographyStyle Heading3 { get; set; } = new TypographyStyle();
        public TypographyStyle Body { get; set; } = new TypographyStyle();
        public TypographyStyle Caption { get; set; } = new TypographyStyle();
        public TypographyStyle Button { get; set; } = new TypographyStyle();
    }

    /// <summary>
    /// Represents spacing values
    /// </summary>
    public class SpacingSystem
    {
        public double Xs { get; set; } = 4;
        public double Sm { get; set; } = 8;
        public double Md { get; set; } = 16;
        public double Lg { get; set; } = 24;
        public double Xl { get; set; } = 32;
        public double Xxl { get; set; } = 48;
    }

    /// <summary>
    /// Represents component styles
    /// </summary>
    public class ComponentStyle
    {
        public string Background { get; set; } = string.Empty;
        public string Border { get; set; } = string.Empty;
        public double BorderRadius { get; set; }
        public string Shadow { get; set; } = string.Empty;
        public string Padding { get; set; } = string.Empty;
        public string Margin { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents all component styles
    /// </summary>
    public class ComponentStyles
    {
        public ComponentStyle Button { get; set; } = new ComponentStyle();
        public ComponentStyle Card { get; set; } = new ComponentStyle();
        public ComponentStyle Input { get; set; } = new ComponentStyle();
        public ComponentStyle Navigation { get; set; } = new ComponentStyle();
        public ComponentStyle Modal { get; set; } = new ComponentStyle();
    }

    /// <summary>
    /// Represents theme types
    /// </summary>
    public enum ThemeType
    {
        Light,
        Dark,
        HighContrast
    }

    /// <summary>
    /// Represents a complete theme
    /// </summary>
    public class Theme
    {
        public ThemeType Type { get; set; }
        public Color Background { get; set; } = new Color();
        public Color Surface { get; set; } = new Color();
        public Color Text { get; set; } = new Color();
        public Color TextSecondary { get; set; } = new Color();
        public ColorPalette Colors { get; set; } = new ColorPalette();
    }

    /// <summary>
    /// Represents responsive breakpoints
    /// </summary>
    public class Breakpoints
    {
        public double Mobile { get; set; } = 768;
        public double Tablet { get; set; } = 1024;
        public double Desktop { get; set; } = 1440;
        public double LargeDesktop { get; set; } = 1920;
    }

    /// <summary>
    /// Represents animation durations
    /// </summary>
    public class AnimationDurations
    {
        public double Fast { get; set; } = 150;
        public double Normal { get; set; } = 300;
        public double Slow { get; set; } = 500;
    }

    /// <summary>
    /// Represents border radius values
    /// </summary>
    public class BorderRadius
    {
        public double Small { get; set; } = 4;
        public double Medium { get; set; } = 8;
        public double Large { get; set; } = 16;
    }

    /// <summary>
    /// Represents shadow styles
    /// </summary>
    public class ShadowStyle
    {
        public double OffsetX { get; set; }
        public double OffsetY { get; set; }
        public double BlurRadius { get; set; }
        public double SpreadRadius { get; set; }
        public string Color { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents all shadow styles
    /// </summary>
    public class Shadows
    {
        public ShadowStyle Small { get; set; } = new ShadowStyle();
        public ShadowStyle Medium { get; set; } = new ShadowStyle();
        public ShadowStyle Large { get; set; } = new ShadowStyle();
    }

    /// <summary>
    /// Represents accessibility features
    /// </summary>
    public class AccessibilityFeatures
    {
        public string FocusIndicator { get; set; } = string.Empty;
        public string HighContrast { get; set; } = string.Empty;
        public string ScreenReader { get; set; } = string.Empty;
        public string KeyboardNavigation { get; set; } = string.Empty;
    }

    /// <summary>
    /// Modern design system for the Flashcard App
    /// </summary>
    public class DesignSystem
    {
        public ColorPalette GetColorPalette()
        {
            return new ColorPalette
            {
                Primary = new Color { Name = "Primary", Hex = "#0078D4", R = 0, G = 120, B = 212 },
                Secondary = new Color { Name = "Secondary", Hex = "#106EBE", R = 16, G = 110, B = 190 },
                Success = new Color { Name = "Success", Hex = "#107C10", R = 16, G = 124, B = 16 },
                Warning = new Color { Name = "Warning", Hex = "#FF8C00", R = 255, G = 140, B = 0 },
                Error = new Color { Name = "Error", Hex = "#D13438", R = 209, G = 52, B = 56 },
                Info = new Color { Name = "Info", Hex = "#0078D4", R = 0, G = 120, B = 212 },
                Neutral = new Color { Name = "Neutral", Hex = "#605E5C", R = 96, G = 94, B = 92 }
            };
        }

        public TypographyScale GetTypographyScale()
        {
            return new TypographyScale
            {
                Heading1 = new TypographyStyle
                {
                    FontFamily = "Segoe UI",
                    FontSize = 32,
                    LineHeight = 40,
                    FontWeight = "Bold",
                    FontStyle = "Normal"
                },
                Heading2 = new TypographyStyle
                {
                    FontFamily = "Segoe UI",
                    FontSize = 24,
                    LineHeight = 32,
                    FontWeight = "SemiBold",
                    FontStyle = "Normal"
                },
                Heading3 = new TypographyStyle
                {
                    FontFamily = "Segoe UI",
                    FontSize = 20,
                    LineHeight = 28,
                    FontWeight = "SemiBold",
                    FontStyle = "Normal"
                },
                Body = new TypographyStyle
                {
                    FontFamily = "Segoe UI",
                    FontSize = 14,
                    LineHeight = 20,
                    FontWeight = "Normal",
                    FontStyle = "Normal"
                },
                Caption = new TypographyStyle
                {
                    FontFamily = "Segoe UI",
                    FontSize = 12,
                    LineHeight = 16,
                    FontWeight = "Normal",
                    FontStyle = "Normal"
                },
                Button = new TypographyStyle
                {
                    FontFamily = "Segoe UI",
                    FontSize = 14,
                    LineHeight = 20,
                    FontWeight = "SemiBold",
                    FontStyle = "Normal"
                }
            };
        }

        public SpacingSystem GetSpacingSystem()
        {
            return new SpacingSystem
            {
                Xs = 4,
                Sm = 8,
                Md = 16,
                Lg = 24,
                Xl = 32,
                Xxl = 48
            };
        }

        public ComponentStyles GetComponentStyles()
        {
            return new ComponentStyles
            {
                Button = new ComponentStyle
                {
                    Background = "#0078D4",
                    Border = "1px solid #0078D4",
                    BorderRadius = 4,
                    Shadow = "0 2px 4px rgba(0,0,0,0.1)",
                    Padding = "8px 16px",
                    Margin = "4px"
                },
                Card = new ComponentStyle
                {
                    Background = "#FFFFFF",
                    Border = "1px solid #E1DFDD",
                    BorderRadius = 8,
                    Shadow = "0 4px 8px rgba(0,0,0,0.1)",
                    Padding = "16px",
                    Margin = "8px"
                },
                Input = new ComponentStyle
                {
                    Background = "#FFFFFF",
                    Border = "1px solid #D2D0CE",
                    BorderRadius = 4,
                    Shadow = "none",
                    Padding = "8px 12px",
                    Margin = "4px"
                },
                Navigation = new ComponentStyle
                {
                    Background = "#F3F2F1",
                    Border = "1px solid #E1DFDD",
                    BorderRadius = 0,
                    Shadow = "0 2px 4px rgba(0,0,0,0.1)",
                    Padding = "16px",
                    Margin = "0"
                },
                Modal = new ComponentStyle
                {
                    Background = "#FFFFFF",
                    Border = "1px solid #E1DFDD",
                    BorderRadius = 8,
                    Shadow = "0 8px 16px rgba(0,0,0,0.2)",
                    Padding = "24px",
                    Margin = "0"
                }
            };
        }

        public Theme GetTheme(ThemeType type)
        {
            var colorPalette = GetColorPalette();

            return type switch
            {
                ThemeType.Light => new Theme
                {
                    Type = ThemeType.Light,
                    Background = new Color { Name = "Background", Hex = "#FFFFFF", R = 255, G = 255, B = 255 },
                    Surface = new Color { Name = "Surface", Hex = "#F3F2F1", R = 243, G = 242, B = 241 },
                    Text = new Color { Name = "Text", Hex = "#323130", R = 50, G = 49, B = 48 },
                    TextSecondary = new Color { Name = "TextSecondary", Hex = "#605E5C", R = 96, G = 94, B = 92 },
                    Colors = colorPalette
                },
                ThemeType.Dark => new Theme
                {
                    Type = ThemeType.Dark,
                    Background = new Color { Name = "Background", Hex = "#1E1E1E", R = 30, G = 30, B = 30 },
                    Surface = new Color { Name = "Surface", Hex = "#2D2D2D", R = 45, G = 45, B = 45 },
                    Text = new Color { Name = "Text", Hex = "#FFFFFF", R = 255, G = 255, B = 255 },
                    TextSecondary = new Color { Name = "TextSecondary", Hex = "#C8C6C4", R = 200, G = 198, B = 196 },
                    Colors = colorPalette
                },
                ThemeType.HighContrast => new Theme
                {
                    Type = ThemeType.HighContrast,
                    Background = new Color { Name = "Background", Hex = "#000000", R = 0, G = 0, B = 0 },
                    Surface = new Color { Name = "Surface", Hex = "#000000", R = 0, G = 0, B = 0 },
                    Text = new Color { Name = "Text", Hex = "#FFFFFF", R = 255, G = 255, B = 255 },
                    TextSecondary = new Color { Name = "TextSecondary", Hex = "#FFFFFF", R = 255, G = 255, B = 255 },
                    Colors = colorPalette
                },
                _ => throw new ArgumentException($"Unknown theme type: {type}")
            };
        }

        public Breakpoints GetBreakpoints()
        {
            return new Breakpoints
            {
                Mobile = 768,
                Tablet = 1024,
                Desktop = 1440,
                LargeDesktop = 1920
            };
        }

        public AnimationDurations GetAnimationDurations()
        {
            return new AnimationDurations
            {
                Fast = 150,
                Normal = 300,
                Slow = 500
            };
        }

        public BorderRadius GetBorderRadius()
        {
            return new BorderRadius
            {
                Small = 4,
                Medium = 8,
                Large = 16
            };
        }

        public Shadows GetShadows()
        {
            return new Shadows
            {
                Small = new ShadowStyle
                {
                    OffsetX = 0,
                    OffsetY = 2,
                    BlurRadius = 4,
                    SpreadRadius = 0,
                    Color = "rgba(0,0,0,0.1)"
                },
                Medium = new ShadowStyle
                {
                    OffsetX = 0,
                    OffsetY = 4,
                    BlurRadius = 8,
                    SpreadRadius = 0,
                    Color = "rgba(0,0,0,0.15)"
                },
                Large = new ShadowStyle
                {
                    OffsetX = 0,
                    OffsetY = 8,
                    BlurRadius = 16,
                    SpreadRadius = 0,
                    Color = "rgba(0,0,0,0.2)"
                }
            };
        }

        public double ValidateColorContrast(Color foreground, Color background)
        {
            // Calculate relative luminance
            var fgLuminance = CalculateRelativeLuminance(foreground);
            var bgLuminance = CalculateRelativeLuminance(background);

            // Calculate contrast ratio
            var lighter = Math.Max(fgLuminance, bgLuminance);
            var darker = Math.Min(fgLuminance, bgLuminance);

            return (lighter + 0.05) / (darker + 0.05);
        }

        public Dictionary<string, string> GenerateResponsiveStyles(Breakpoints breakpoints)
        {
            return new Dictionary<string, string>
            {
                ["mobile"] = $"@media (max-width: {breakpoints.Mobile}px)",
                ["tablet"] = $"@media (min-width: {breakpoints.Mobile + 1}px) and (max-width: {breakpoints.Tablet}px)",
                ["desktop"] = $"@media (min-width: {breakpoints.Tablet + 1}px) and (max-width: {breakpoints.Desktop}px)",
                ["large-desktop"] = $"@media (min-width: {breakpoints.Desktop + 1}px)"
            };
        }

        public AccessibilityFeatures GetAccessibilityFeatures()
        {
            return new AccessibilityFeatures
            {
                FocusIndicator = "2px solid #0078D4",
                HighContrast = "High contrast mode support",
                ScreenReader = "ARIA labels and semantic markup",
                KeyboardNavigation = "Full keyboard navigation support"
            };
        }

        private double CalculateRelativeLuminance(Color color)
        {
            // Convert RGB to relative luminance
            var r = color.R / 255.0;
            var g = color.G / 255.0;
            var b = color.B / 255.0;

            // Apply gamma correction
            r = r <= 0.03928 ? r / 12.92 : Math.Pow((r + 0.055) / 1.055, 2.4);
            g = g <= 0.03928 ? g / 12.92 : Math.Pow((g + 0.055) / 1.055, 2.4);
            b = b <= 0.03928 ? b / 12.92 : Math.Pow((b + 0.055) / 1.055, 2.4);

            return 0.2126 * r + 0.7152 * g + 0.0722 * b;
        }
    }
}
