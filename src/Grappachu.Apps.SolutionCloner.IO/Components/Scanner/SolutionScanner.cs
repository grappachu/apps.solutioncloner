using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Grappachu.Apps.SolutionCloner.Engine.Components.Scanner
{
    public class SolutionScanner
    {
        public IEnumerable<SolutionFile> FindAll(string root)
        {
            var slnFiles = Directory.GetFiles(root, "*.sln", SearchOption.AllDirectories);
            return slnFiles.Select(s => new SolutionFile(s));
        }
    }
}