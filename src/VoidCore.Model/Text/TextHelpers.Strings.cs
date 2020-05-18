using System;

namespace VoidCore.Model.Text
{
    public static partial class TextHelpers
    {
        /// <summary>
        /// Split a string on any newline character.
        /// </summary>
        /// <param name="str">The string to split</param>
        public static string[] SplitOnNewLine(string str)
        {
            return str.Split(new[] { "\n", "\r\n" }, StringSplitOptions.None);
        }
    }
}
