using System.IO;
using System.Reflection;
using System.Runtime.Loader;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

using SimpleWpf.Extensions.Collection;

namespace SimpleNotepad.Component
{
    public static class CSharpCompiler
    {
        // This will serve to get an assembly to use for our line-by-line processing of input text
        //
        const string CODE_TEMPLATE = @"
        using System;

        namespace SimpleNotepadUserMethods
        {
            public class UserMethods
            {
                // Our Method Goes Here!
                {0}
            }
        }";


        public static Assembly Compile(string source, out IEnumerable<MethodInfo> publicMethods, out IEnumerable<Diagnostic> diagnostics, out string errorMessage)
        {
            var diagnosticResult = new List<Diagnostic>();

            diagnostics = diagnosticResult;
            errorMessage = string.Empty;
            publicMethods = null;

            try
            {
                // Insert our method into the source template
                var codeToCompile = CODE_TEMPLATE.Replace("{0}", source);

                // Parse the text into a syntax tree
                var syntaxTree = CSharpSyntaxTree.ParseText(codeToCompile);

                // Ook.
                string assemblyName = Path.GetRandomFileName();
                var refPaths = new[] {
                        typeof(System.Object).GetTypeInfo().Assembly.Location,
                        typeof(Console).GetTypeInfo().Assembly.Location,
                        Path.Combine(Path.GetDirectoryName(typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly.Location), "System.Runtime.dll")
                    };
                MetadataReference[] references = refPaths.Select(r => MetadataReference.CreateFromFile(r)).ToArray();

                CSharpCompilation compilation = CSharpCompilation.Create(
                    assemblyName,
                    syntaxTrees: new[] { syntaxTree },
                    references: references,
                    options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

                using (var ms = new MemoryStream())
                {
                    EmitResult result = compilation.Emit(ms);

                    diagnosticResult.AddRange(result.Diagnostics);

                    if (!result.Success)
                    {
                        return null;
                    }
                    else
                    {
                        ms.Seek(0, SeekOrigin.Begin);

                        var assembly = AssemblyLoadContext.Default.LoadFromStream(ms);
                        var type = assembly.GetType("SimpleNotepadUserMethods.UserMethods");
                        publicMethods = type.GetMethods();

                        return assembly;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return null;
            }
        }
    }
}
