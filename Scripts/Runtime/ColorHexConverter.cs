using UnityEngine;

namespace AdvancedDebugger
{
    internal static class ColorHexConverter
    {
        internal static string GetHexColor(System.Drawing.Color color, bool useAlpha = false)
        {
            string result = $"{color.R:X2}{color.G:X2}{color.B:X2}";
            return useAlpha ? $"{color.A:X2}" : result;
        }

        internal static string GetHexColor(Color color, bool useAlpha = false)
        {
            return useAlpha ? ColorUtility.ToHtmlStringRGBA(color) : ColorUtility.ToHtmlStringRGB(color);
        }
    }
}
