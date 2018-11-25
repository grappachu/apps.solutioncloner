using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Grappachu.Apps.SolutionCloner.Engine.Components.Scanner
{
    public class ProjectInfo
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public string RelativePath { get; set; }
        public Guid Id { get; set; }

        public string GetNamespace()
        {
            var csFiles = Directory.GetFiles(Path.GetDirectoryName(FullPath), "*.cs", SearchOption.AllDirectories);
            var ns = new List<string>();

            foreach (var csFile in csFiles)
            {
                var lines = File.ReadAllLines(csFile).Where(x => x.StartsWith("namespace "));
                foreach (var line in lines)
                {
                    var name = line.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries).ElementAtOrDefault(1);
                    if (!string.IsNullOrWhiteSpace(name))
                        ns.Add(name);
                }
            }

            return ns.OrderBy(l => l.Length).FirstOrDefault();
        }
    }
}