using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Grappachu.SolutionCloner.Engine.Components.Scanner
{
    internal class SolutionScanner
    {
        public IEnumerable<SolutionFile> FindAll(string root)
        {
            var slnFiles = Directory.GetFiles(root, "*.sln", SearchOption.AllDirectories);
            return slnFiles.Select(s => new SolutionFile(s));
        }
    }
}