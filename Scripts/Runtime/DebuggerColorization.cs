using System;
using System.Drawing;

namespace AdvancedDebugger
{
    public class DebuggerColorization
    {
        public string Name { get; private set; }
        public string HexColor { get; private set; }

        private const char SharpSymbol = '#';

        public DebuggerColorization(string name, Color color) : this(name, ColorHexConverter.GetHexColor(color)) { }
        public DebuggerColorization(string name, UnityEngine.Color color) : this(name, ColorHexConverter.GetHexColor(color)) { }
        public DebuggerColorization(string name) : this(name, DebuggerConstants.DefaultColor) { }

        public DebuggerColorization(string name, string hexColor)
        {
            Name = name;
            hexColor = AddSharpIfNeed(hexColor);
            HexColor = hexColor;
        }

        private static string AddSharpIfNeed(string hexColor)
        {
            if (hexColor[0] != SharpSymbol)
            {
                hexColor = $"{SharpSymbol}{hexColor}";
            }

            return hexColor;
        }
    }
}