using System.Drawing;

namespace AdvancedDebugger
{
    internal static class ColorHexConverter
    {
        internal static string GetStringFromColor(Color color, bool useAlpha = false)
        {
            string result = $"{color.R:X2}{color.G:X2}{color.B:X2}";
            return useAlpha ? $"{color.A:X2}" : result;
        }
    }
}
