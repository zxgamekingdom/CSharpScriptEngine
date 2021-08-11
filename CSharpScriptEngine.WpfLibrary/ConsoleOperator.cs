using System;
using System.Runtime.InteropServices;

namespace CSharpScriptEngine.WpfLibrary
{
    public static class ConsoleOperator
    {
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();
    }
}
