using System;
using CSharpScriptEngine.Library.Results;
using Microsoft.CodeAnalysis;

namespace CSharpScriptEngine.Library.Options
{
    public class DiagnosticSyntaxTreeOptions
    {
        public static implicit operator DiagnosticSyntaxTreeOptions(
            SourceCodeResults sourceCodeResults) =>
            new(sourceCodeResults.CodePath, sourceCodeResults.SyntaxTree);

        public DiagnosticSyntaxTreeOptions(string codePath, SyntaxTree syntaxTree)
        {
            if (string.IsNullOrWhiteSpace(codePath))
            {
                throw new ArgumentException("Value cannot be null or whitespace.",
                    nameof(codePath));
            }

            CodePath = codePath ?? throw new ArgumentNullException(nameof(codePath));
            SyntaxTree = syntaxTree ??
                throw new ArgumentNullException(nameof(syntaxTree));
        }

        public string CodePath { get; }
        public SyntaxTree SyntaxTree { get; }
    }
}
