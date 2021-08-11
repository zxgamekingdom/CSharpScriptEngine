using System;

namespace CSharpScriptEngine.Library.Extensions
{
    public static class ConsoleExtensions
    {
        public static void WriteLine<T>(this T t,
            ConsoleColor foregroundColor = ConsoleColor.Gray,
            ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            lock (Console.Out)
            {
                ConsoleColor backgroundBuff = Console.BackgroundColor;
                ConsoleColor foregroundBuff = Console.ForegroundColor;
                Console.BackgroundColor = backgroundColor;
                Console.ForegroundColor = foregroundColor;
                Console.WriteLine(t);
                Console.BackgroundColor = backgroundBuff;
                Console.ForegroundColor = foregroundBuff;
            }
        }

        public static void ConsoleSplitLine(char splitLineChar = '_',
            ConsoleColor foregroundColor = ConsoleColor.Gray,
            ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            int width = Console.WindowWidth;
            new string(splitLineChar, width - 1).WriteLine(foregroundColor,
                backgroundColor);
        }
    }
}
