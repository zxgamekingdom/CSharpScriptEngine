using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace CSharpScriptEngine.Library.Results
{
    public class DiagnosticSyntaxTreeResults
    {
        private readonly Diagnostic[]? _diagnostics;

        public DiagnosticSyntaxTreeResults(string codePath,
            IEnumerable<Diagnostic>? diagnostics)
        {
            if (string.IsNullOrWhiteSpace(codePath))
            {
                throw new ArgumentException("Value cannot be null or whitespace.",
                    nameof(codePath));
            }

            CodePath = codePath ?? throw new ArgumentNullException(nameof(codePath));
            if (diagnostics != null)
                _diagnostics = diagnostics as Diagnostic[] ?? diagnostics.ToArray();
        }

        public string CodePath { get; }

        public bool ExistDiagnostics =>
            _diagnostics switch
            {
                {Length: >0} => true,
                _ => false
            };

        public IEnumerable<Diagnostic>? Diagnostics => _diagnostics;
    }
}
