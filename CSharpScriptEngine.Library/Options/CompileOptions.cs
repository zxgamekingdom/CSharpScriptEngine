using System;
using System.Collections.Generic;
using System.Linq;
using CSharpScriptEngine.Library.Extensions;
using CSharpScriptEngine.Library.Results;
using Microsoft.CodeAnalysis;

namespace CSharpScriptEngine.Library.Options
{
    public class CompileOptions
    {
        private readonly string? _assemblyName;
        private readonly List<PortableExecutableReference> _references = new();
        private readonly List<SourceCodeCompileOptions> _compileCodeArgs = new();

        public CompileOptions(string? assemblyName = null,
            IEnumerable<SourceCodeCompileOptions>? sourceCodeCompileArgs = null,
            IEnumerable<PortableExecutableReference>? references = null)
        {
            _assemblyName = assemblyName;
            if (sourceCodeCompileArgs is { })
                _compileCodeArgs.AddRange(sourceCodeCompileArgs);
            if (references is { }) _references.AddRange(references);
        }

        public string AssemblyName =>
            _assemblyName switch
            {
                { } s when s.IsNotNullOrWhiteSpace() => s,
                _ => Guid.NewGuid().ToString()
            };

        public IEnumerable<PortableExecutableReference> References =>
            _references switch
            {
                {Count: > 0} => _references,
                _ => throw new EmptyCollectionException()
            };

        public IEnumerable<SyntaxTree> SyntaxTrees =>
            _compileCodeArgs switch
            {
                {Count: > 0} => CompileCodeArgs.Select(arg => arg.SyntaxTree),
                _ => throw new EmptyCollectionException()
            };

        public IEnumerable<SourceCodeCompileOptions> CompileCodeArgs => _compileCodeArgs;

        public CompileOptions Add(IEnumerable<SourceCodeCompileOptions> args)
        {
            _compileCodeArgs.AddRange(args);
            return this;
        }

        public CompileOptions Add(IEnumerable<SourceCodeResults> results)
        {
            SourceCodeResults[] buff =
                results as SourceCodeResults[] ?? results.ToArray();
            int length = buff.Length;
            var arr = new SourceCodeCompileOptions[length];
            for (var i = 0; i < length; i++) arr[i] = buff[i];
            _compileCodeArgs.AddRange(arr);
            return this;
        }

        public CompileOptions Add(IEnumerable<PortableExecutableReference> references)
        {
            _references.AddRange(references);
            return this;
        }

        public CompileOptions Add(IEnumerable<string> dllPath)
        {
            _references.AddRange(dllPath.Select(s =>
                MetadataReference.CreateFromFile(s)));
            return this;
        }

        public CompileOptions Add(IEnumerable<Type> types)
        {
            _references.AddRange(types.Select(type =>
                MetadataReference.CreateFromFile(type.Assembly.Location)));
            return this;
        }
    }
}
