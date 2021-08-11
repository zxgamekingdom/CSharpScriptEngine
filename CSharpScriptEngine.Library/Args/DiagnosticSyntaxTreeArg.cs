using System;
using CSharpScriptEngine.Library.Results;
using Microsoft.CodeAnalysis;

namespace CSharpScriptEngine.Library.Args
{
    public class DiagnosticSyntaxTreeArg
    {
        public static implicit operator DiagnosticSyntaxTreeArg(
            SourceCodeResult sourceCodeResult) =>
            new(sourceCodeResult.CodePath, sourceCodeResult.SyntaxTree);

        public DiagnosticSyntaxTreeArg(string codePath, SyntaxTree syntaxTree)
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
