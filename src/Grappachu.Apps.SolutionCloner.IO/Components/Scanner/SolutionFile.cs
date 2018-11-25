using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Grappachu.Core.Lang.Extensions;

namespace Grappachu.Apps.SolutionCloner.Engine.Components.Scanner
{
    public class SolutionFile
    {
        private readonly string _solutionFile;

        public SolutionFile(string solutionFile)
        {
            _solutionFile = solutionFile;
        }

        public IEnumerable<ProjectInfo> GetProjects()
        {
            var sn = Path.GetFileNameWithoutExtension(_solutionFile);
            var solutionRoot = (string)Path.GetDirectoryName(_solutionFile).OrDie("Invalid Path");

            List<ProjectInfo> li = new List<ProjectInfo>();
            var lines = File.ReadAllLines(_solutionFile).Where(l => l.StartsWith("Project") && l.Contains(".csproj"));
            foreach (var line in lines)
            {
                var lineParts = line.Split('=').ToArray();
                var guid = new Guid(lineParts.ElementAt(0).Split('"').ElementAt(1));
                var pParts = lineParts.ElementAt(1).Split(',').Select(p => p.Replace('"', ' ').Trim()).ToArray();
                var relPath = pParts[1];
                ProjectInfo prj = new ProjectInfo
                {
                    Id = guid,
                    Name = pParts[0],
                    RelativePath = relPath,
                    FullPath = Path.Combine(solutionRoot, relPath)
                };
                li.Add(prj);
            }

            var candidates = li.OrderBy(p => Core.Lang.Text.LevenshteinDistance.ComputeIgnoreCase(p.Name, sn));

            return candidates;
        }
    }

    

}