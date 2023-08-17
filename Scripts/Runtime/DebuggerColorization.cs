using System.Drawing;

namespace AdvancedDebugger
{
    public class DebuggerColorization
    {
        public string Name { get; private set; }
        public string HexColor { get; private set; }

        private const char SharpSymbol = '#';

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Caller (Class) name</param>
        /// <param name="hexColor">Hex color format: #FFFFFF</param>
        public DebuggerColorization(string name, string hexColor = DebuggerConstants.DefaultColor)
        {
            Name = name;
            hexColor = AddSharpIfNeed(hexColor);
            HexColor = hexColor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Caller (Class) name</param>
        /// <param name="color"></param>
        public DebuggerColorization(string name, Color color) : this(name, ColorHexConverter.GetHexColor(color)) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Caller (Class) name</param>
        /// <param name="color"></param>
        public DebuggerColorization(string name, UnityEngine.Color color) : this(name, ColorHexConverter.GetHexColor(color)) { }

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