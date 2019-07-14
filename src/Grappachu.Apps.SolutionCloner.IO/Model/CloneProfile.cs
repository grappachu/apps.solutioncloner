using System.Collections.Generic;

namespace Grappachu.Apps.SolutionCloner.Engine.Model
{
    public class CloneProfile
    {
        public CloneProfile()
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
                ".xml",
                ".aspx",
                ".ashx",
                ".asax",
                ".xaml",
                ".vb",
                ".vbproj",
                ".json",
                ".js",
                ".css",
                ".cshtml",
                ".yml"
            });
            DeleteFiles = new List<string>(new[]
            {
                ".suo",
                ".DotSettings",
                ".pdb"
            });
            ExcludeFolders = new List<string>(new[]
            {
                "\\bin\\",
                "\\obj\\",
                "\\packages\\"
            });
            ExtraReplacements = new List<CloneReplacement>();
        }


        public IList<string> ReplaceFiles { get; set; }
        public IList<string> DeleteFiles { get; set; }
        public IList<string> ExcludeFolders { get; set; }

        public IList<CloneReplacement> ExtraReplacements { get; }
    }
}