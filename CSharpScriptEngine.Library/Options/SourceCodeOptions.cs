using System;
using CSharpScriptEngine.Library.Extensions;

namespace CSharpScriptEngine.Library.Options
{
    public class SourceCodeOptions
    {
        private readonly string? _fileName;

        public SourceCodeOptions(string codeString, string? fileName = null)
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
