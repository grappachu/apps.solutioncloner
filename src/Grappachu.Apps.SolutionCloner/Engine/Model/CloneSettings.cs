using System.Collections.Generic;
using System.IO;

namespace Grappachu.SolutionCloner.Engine.Model
{
    internal class CloneSettings
    {
        public CloneSettings()
        {
            ReplaceFiles = new List<string>(new[]
            {
                ".sln",
                ".csproj",
                ".cs",
                ".config",
                ".resx",
                ".md",
                ".gitignore",
                ".nuspec",
                ".xml"
            });
        }

        public DirectoryInfo TargetFolder { get; set; }
        public DirectoryInfo TemplateSource { get; set; }

        public string TemplateKey { get; set; }

        public string TargetKey { get; set; }

        public IList<string> ReplaceFiles { get; }
    }
}