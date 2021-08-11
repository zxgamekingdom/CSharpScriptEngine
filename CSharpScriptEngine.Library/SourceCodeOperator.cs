using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using CSharpScriptEngine.Library.Args;
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
        public static IEnumerable<SourceCodeResult> GetSourceTextAndSyntaxTree(
            IEnumerable<SourceCodeArg> args)
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
                return new SourceCodeResult(codePath, sourceText, syntaxTree);
            });
        }

        /// <summary>
        /// 诊断语法树是否有问题
        /// </summary>
        public static IEnumerable<DiagnosticSyntaxTreeResult> DiagnosticSyntaxTree(
            IEnumerable<DiagnosticSyntaxTreeArg> args)
        {
            return args.Select(x =>
            {
                Diagnostic[] buff = x.SyntaxTree.GetDiagnostics().ToArray();
                return buff.Length == 0 ?
                    new DiagnosticSyntaxTreeResult(x.CodePath, null) :
                    new DiagnosticSyntaxTreeResult(x.CodePath, buff);
            });
        }

        /// <summary>
        /// 诊断语法树是否有问题
        /// </summary>
        public static IEnumerable<DiagnosticSyntaxTreeResult> DiagnosticSyntaxTree(
            IEnumerable<SourceCodeResult> codeResults)
        {
            SourceCodeResult[] buffArr =
                codeResults as SourceCodeResult[] ?? codeResults.ToArray();
            int length = buffArr.Length;
            DiagnosticSyntaxTreeArg[] arr = new DiagnosticSyntaxTreeArg[length];
            for (var i = 0; i < length; i++) arr[i] = buffArr[i];
            return DiagnosticSyntaxTree(arr);
        }

        /// <summary>
        /// 编译
        /// </summary>
        public static CompileResult Compile(CompileArg arg)
        {
            string assemblyName = arg.AssemblyName;
            var compilation = CSharpCompilation.Create(assemblyName,
                arg.SyntaxTrees,
                arg.References,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            using var pdbStream = new MemoryStream();
            using var peStream = new MemoryStream();
            EmitResult emitResult = compilation.Emit(peStream,
                pdbStream,
                options: new EmitOptions(
                    debugInformationFormat: DebugInformationFormat.PortablePdb),
                embeddedTexts: arg.CompileCodeArgs.Select(x =>
                    EmbeddedText.FromSource(x.CodePath, x.SourceText)));
            bool success = emitResult.Success;
            ImmutableArray<Diagnostic> diagnostics = emitResult.Diagnostics;
            return success switch
            {
                true => new CompileResult(true,
                    arg,
                    pdbStream.ToArray(),
                    peStream.ToArray(),
                    diagnostics),
                false => new CompileResult(false,
                    arg,
                    diagnostics: diagnostics)
            };
        }
    }
}
