using System;
using System.Collections.Generic;
using System.Linq;
using CSharpScriptEngine.Library.Extensions;
using CSharpScriptEngine.Library.Results;
using Microsoft.CodeAnalysis;

namespace CSharpScriptEngine.Library.Args
{
    public class CompileArg
    {
        private readonly string? _assemblyName;
        private readonly List<PortableExecutableReference> _references = new();
        private readonly List<SourceCodeCompileArg> _compileCodeArgs = new();

        public CompileArg(string? assemblyName = null,
            IEnumerable<SourceCodeCompileArg>? sourceCodeCompileArgs = null,
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

        public IEnumerable<SourceCodeCompileArg> CompileCodeArgs => _compileCodeArgs;

        public CompileArg Add(IEnumerable<SourceCodeCompileArg> args)
        {
            _compileCodeArgs.AddRange(args);
            return this;
        }

        public CompileArg Add(IEnumerable<SourceCodeResult> results)
        {
            SourceCodeResult[] buff =
                results as SourceCodeResult[] ?? results.ToArray();
            int length = buff.Length;
            var arr = new SourceCodeCompileArg[length];
            for (var i = 0; i < length; i++) arr[i] = buff[i];
            _compileCodeArgs.AddRange(arr);
            return this;
        }

        public CompileArg Add(IEnumerable<PortableExecutableReference> references)
        {
            _references.AddRange(references);
            return this;
        }

        public CompileArg Add(IEnumerable<string> dllPath)
        {
            _references.AddRange(dllPath.Select(s =>
                MetadataReference.CreateFromFile(s)));
            return this;
        }

        public CompileArg Add(IEnumerable<Type> types)
        {
            _references.AddRange(types.Select(type =>
                MetadataReference.CreateFromFile(type.Assembly.Location)));
            return this;
        }
    }
}
