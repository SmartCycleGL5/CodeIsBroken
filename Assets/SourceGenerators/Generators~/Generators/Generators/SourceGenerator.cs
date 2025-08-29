using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using System.CodeDom.Compiler;
using System.IO;

namespace Generators;

[Generator]
public class SourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var compilationProvider = context.CompilationProvider;
        
        context.RegisterSourceOutput(compilationProvider, (ctx, compilation) =>
        {
            if (compilation.AssemblyName == "Assembly-CSharp")
            {
                ctx.AddSource("Example.g.cs", GenerateSource());
            }
        });
    }
}