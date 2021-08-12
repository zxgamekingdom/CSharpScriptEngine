using System;
using CSharpScriptEngine.Library.Results;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace CSharpScriptEngine.Library.Options
{
    public class SourceCodeCompileOptions
    {
        public static implicit operator SourceCodeCompileOptions(SourceCodeResults results)
        {
            return new(results.CodePath, results.SourceText, results.SyntaxTree);
        }

        public SourceCodeCompileOptions(string codePath,
            SourceText sourceText,
            SyntaxTree syntaxTree)
        {
            if (string.IsNullOrWhiteSpace(codePath))
            {
                throw new ArgumentException("Value cannot be null or whitespace.",
                    nameof(codePath));
            }

            CodePath = codePath ?? throw new ArgumentNullException(nameof(codePath));
            SourceText = sourceText ??
                throw new ArgumentNullException(nameof(sourceText));
            SyntaxTree = syntaxTree ??
                throw new ArgumentNullException(nameof(syntaxTree));
        }

        public string CodePath { get; }
        public SourceText SourceText { get; }
        public SyntaxTree SyntaxTree { get; }
    }
}
