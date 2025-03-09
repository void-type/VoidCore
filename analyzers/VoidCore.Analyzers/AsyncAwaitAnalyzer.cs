using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace VoidCore.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class AsyncAwaitAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "AsyncAwaitAnalyzer";

    private static readonly LocalizableString Title = "Use async/await";
    private static readonly LocalizableString MessageFormat = "Method '{0}' should use async/await";
    private static readonly LocalizableString Description = "Ensure that asynchronous methods use async/await.";
    private const string Category = "AsyncUsage";

    private static readonly DiagnosticDescriptor _rule = new(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [_rule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeMethod, SyntaxKind.MethodDeclaration);
    }

    private static void AnalyzeMethod(SyntaxNodeAnalysisContext context)
    {
        var methodDeclaration = (MethodDeclarationSyntax)context.Node;

        // Check if the method is declared in an interface or is abstract
        if (context.SemanticModel.GetDeclaredSymbol(methodDeclaration) is not IMethodSymbol methodSymbol
            || methodSymbol.ContainingType.TypeKind == TypeKind.Interface || methodSymbol.IsAbstract)
        {
            return;
        }

        // Check if the method returns Task or Task<T>
        var returnType = context.SemanticModel.GetTypeInfo(methodDeclaration.ReturnType).Type;
        if (returnType == null || !returnType.Name.StartsWith("Task"))
        {
            return;
        }

        // Check if the method uses async/await
        if (!methodDeclaration.Modifiers.Any(SyntaxKind.AsyncKeyword))
        {
            var diagnostic = Diagnostic.Create(_rule, methodDeclaration.Identifier.GetLocation(), methodDeclaration.Identifier.Text);
            context.ReportDiagnostic(diagnostic);
        }
    }
}
