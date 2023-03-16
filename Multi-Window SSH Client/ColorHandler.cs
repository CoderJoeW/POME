using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POME
{
    internal class ColorHandler
    {
        public static Color GetColorFromSGRCode(int code)
        {
            // Return the corresponding color for the given SGR color code
            switch (code)
            {
                case 30: return Color.Black;
                case 31: return Color.Red;
                case 32: return Color.Green;
                case 33: return Color.Yellow;
                case 34: return Color.Blue;
                case 35: return Color.Magenta;
                case 36: return Color.Cyan;
                case 37: return Color.White;
                case 90: return Color.FromArgb(128, 128, 128); // Bright Black
                case 91: return Color.FromArgb(255, 0, 0); // Bright Red
                case 92: return Color.FromArgb(0, 255, 0); // Bright Green
                case 93: return Color.FromArgb(255, 255, 0); // Bright Yellow
                case 94: return Color.FromArgb(0, 0, 255); // Bright Blue
                case 95: return Color.FromArgb(255, 0, 255); // Bright Magenta
                case 96: return Color.FromArgb(0, 255, 255); // Bright Cyan
                case 97: return Color.FromArgb(255, 255, 255); // Bright White
                default:
                    return Color.Empty;
            }
        }

        public static Color GetExtendedColor(string currentParam, string[] parameters)
        {
            int currentIndex = Array.IndexOf(parameters, currentParam);
            int colorMode;
            int red, green, blue;

            if (currentIndex < 0 || currentIndex + 1 >= parameters.Length)
            {
                return Color.Empty;
            }

            if (int.TryParse(parameters[currentIndex + 1], out colorMode))
            {
                if (colorMode == 2 && currentIndex + 4 < parameters.Length) // RGB mode
                {
                    if (int.TryParse(parameters[currentIndex + 2], out red) &&
                        int.TryParse(parameters[currentIndex + 3], out green) &&
                        int.TryParse(parameters[currentIndex + 4], out blue))
                    {
                        return Color.FromArgb(red, green, blue);
                    }
                }
                else if (colorMode == 5 && currentIndex + 2 < parameters.Length) // 256-color mode
                {
                    int colorIndex;
                    if (int.TryParse(parameters[currentIndex + 2], out colorIndex))
                    {
                        return GetColorFrom256ColorPalette(colorIndex);
                    }
                }
            }

            return Color.Empty;
        }

        public static Color GetColorFrom256ColorPalette(int index)
        {
            if (index < 0 || index >= 256) return Color.Empty;

            if (index < 16) return GetColorFromBasic16ColorPalette(index);

            if (index < 232)
            {
                index -= 16;
                int red = index / 36 * 51, green = (index % 36) / 6 * 51, blue = index % 6 * 51;
                return Color.FromArgb(red, green, blue);
            }

            int gray = (index - 232) * 10 + 8;
            return Color.FromArgb(gray, gray, gray);
        }

        public static Color GetColorFromBasic16ColorPalette(int index)
        {
            // Map the basic 16-color palette indices to the actual colors
            switch (index)
            {
                case 0: return Color.Black;
                case 1: return Color.Maroon;
                case 2: return Color.Green;
                case 3: return Color.Olive;
                case 4: return Color.Navy;
                case 5: return Color.Purple;
                case 6: return Color.Teal;
                case 7: return Color.Silver;
                case 8: return Color.Gray;
                case 9: return Color.Red;
                case 10: return Color.Lime;
                case 11: return Color.Yellow;
                case 12: return Color.Blue;
                case 13: return Color.Magenta;
                case 14: return Color.Cyan;
                case 15: return Color.White;
                default: return Color.Empty;
            }
        }
    }
}
