﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using System.IO;

namespace SharpDox.Build.Roslyn
{
    internal class RoslynLoader
    {
        public Solution LoadSolutionFile(string solutionFile)
        {
            var workspace = MSBuildWorkspace.Create();
            workspace.WorkspaceFailed += Workspace_WorkspaceFailed;
            if (FileIsSolution(solutionFile))
            {
                workspace.OpenSolutionAsync(solutionFile).Wait();
            }
            else if (FileIsProject(solutionFile))
            {
                workspace.OpenProjectAsync(solutionFile).Wait();
            }
            return workspace.CurrentSolution;
        }

        private void Workspace_WorkspaceFailed(object sender, WorkspaceDiagnosticEventArgs e)
        {
            var k = "";
        }

        private static bool FileIsSolution(string pathToSolutionFile)
        {
            var extension = Path.GetExtension(pathToSolutionFile);
            return extension != null && extension.ToUpper().Equals(".SLN");
        }

        private static bool FileIsProject(string pathToSolutionFile)
        {
            var extension = Path.GetExtension(pathToSolutionFile);
            return extension != null && extension.ToUpper().Equals(".CSPROJ");
        }
    }
}
