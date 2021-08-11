using System;
using CSharpScriptEngine.Library.Extensions;

namespace CSharpScriptEngine.Library.Args
{
    public class SourceCodeArg
    {
        private readonly string? _fileName;

        public SourceCodeArg(string codeString, string? fileName = null)
        {
            CodeString = codeString;
            _fileName = fileName;
        }

        public string CodeString { get; }

        public string FileName =>
            _fileName switch
            {
                { } s when s.IsNotNullOrWhiteSpace() => s,
                _ => $"{Guid.NewGuid()}.cs"
            };
    }
}
