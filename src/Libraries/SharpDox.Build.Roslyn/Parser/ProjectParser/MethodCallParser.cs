﻿using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SharpDox.Build.Roslyn.MethodVisitors;

namespace SharpDox.Build.Roslyn.Parser.ProjectParser
{
    internal class MethodCallParser : BaseParser
    {
        internal MethodCallParser(ParserOptions parserOptions) : base(parserOptions) { }

        internal void ParseMethodCalls()
        {
            var namespaces = ParserOptions.SDRepository.GetAllNamespaces();
            foreach (var sdNamespace in namespaces)
            {
                foreach (var sdType in sdNamespace.Types)
                {
                    foreach (var sdMethod in sdType.Methods)
                    {
                        HandleOnItemParseStart(sdMethod.Name);
                        var fileId = ParserOptions.CodeSolution.GetDocumentIdsWithFilePath(sdMethod.Region.FilePath).FirstOrDefault();
                        var file = ParserOptions.CodeSolution.GetDocument(fileId);
                        var syntaxTree = file.GetSyntaxTreeAsync().Result;
                        
                        if (file.Project.Language == "C#")
                        {
                            var methodVisitor = new CSharpMethodVisitor(ParserOptions.SDRepository, sdMethod, sdType, file);
                            var methodSyntaxNode = syntaxTree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>()
                                                    .FirstOrDefault(m => m.Span.Start == sdMethod.Region.Start &&
                                                    m.Span.End == sdMethod.Region.End);
                            if (methodSyntaxNode != null)
                            {
                                methodVisitor.Visit(methodSyntaxNode);
                            }
                        }
                        else if (file.Project.Language == "VBNET")
                        {
                            
                        }
                    }
                }
            }
        }
    }
}
