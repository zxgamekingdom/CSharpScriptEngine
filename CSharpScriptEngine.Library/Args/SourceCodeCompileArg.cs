using System;
using CSharpScriptEngine.Library.Results;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace CSharpScriptEngine.Library.Args
{
    public class SourceCodeCompileArg
    {
        public static implicit operator SourceCodeCompileArg(SourceCodeResult result)
        {
            return new(result.CodePath, result.SourceText, result.SyntaxTree);
        }

        public SourceCodeCompileArg(string codePath,
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
