using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using CSharpScriptEngine.Library.Options;
using CSharpScriptEngine.Library.Results;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;

namespace CSharpScriptEngine.Library
{
    public static class SourceCodeOperator
    {
        /// <summary>
        /// 获取源代码的源文本与语法树
        /// </summary>
        public static IEnumerable<SourceCodeResults> GetSourceTextAndSyntaxTree(
            IEnumerable<SourceCodeOptions> args)
        {
            return args.Select(x =>
            {
                var codePath = x.FileName;
                Encoding encoding = Encoding.UTF8;
                byte[] codeBytes = encoding.GetBytes(x.CodeString);
                SourceText sourceText = SourceText.From(codeBytes,
                    codeBytes.Length,
                    encoding,
                    canBeEmbedded: true);
                SyntaxTree syntaxTree =
                    CSharpSyntaxTree.ParseText(sourceText, path: codePath);
                return new SourceCodeResults(codePath, sourceText, syntaxTree);
            });
        }

        /// <summary>
        /// 诊断语法树是否有问题
        /// </summary>
        public static IEnumerable<DiagnosticSyntaxTreeResults> DiagnosticSyntaxTree(
            IEnumerable<DiagnosticSyntaxTreeOptions> args)
        {
            return args.Select(x =>
            {
                Diagnostic[] buff = x.SyntaxTree.GetDiagnostics().ToArray();
                return buff.Length == 0 ?
                    new DiagnosticSyntaxTreeResults(x.CodePath, null) :
                    new DiagnosticSyntaxTreeResults(x.CodePath, buff);
            });
        }

        /// <summary>
        /// 诊断语法树是否有问题
        /// </summary>
        public static IEnumerable<DiagnosticSyntaxTreeResults> DiagnosticSyntaxTree(
            IEnumerable<SourceCodeResults> codeResults)
        {
            SourceCodeResults[] buffArr =
                codeResults as SourceCodeResults[] ?? codeResults.ToArray();
            int length = buffArr.Length;
            DiagnosticSyntaxTreeOptions[] arr = new DiagnosticSyntaxTreeOptions[length];
            for (var i = 0; i < length; i++) arr[i] = buffArr[i];
            return DiagnosticSyntaxTree(arr);
        }

        /// <summary>
        /// 编译
        /// </summary>
        public static CompileResults Compile(CompileOptions options)
        {
            string assemblyName = options.AssemblyName;
            var compilation = CSharpCompilation.Create(assemblyName,
                options.SyntaxTrees,
                options.References,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            using var pdbStream = new MemoryStream();
            using var peStream = new MemoryStream();
            EmitResult emitResult = compilation.Emit(peStream,
                pdbStream,
                options: new EmitOptions(
                    debugInformationFormat: DebugInformationFormat.PortablePdb),
                embeddedTexts: options.CompileCodeArgs.Select(x =>
                    EmbeddedText.FromSource(x.CodePath, x.SourceText)));
            bool success = emitResult.Success;
            ImmutableArray<Diagnostic> diagnostics = emitResult.Diagnostics;
            return success switch
            {
                true => new CompileResults(true,
                    options,
                    pdbStream.ToArray(),
                    peStream.ToArray(),
                    diagnostics),
                false => new CompileResults(false,
                    options,
                    diagnostics: diagnostics)
            };
        }
    }
}
