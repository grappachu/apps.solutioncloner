using System.Collections.Generic;

namespace Grappachu.SolutionCloner.Engine.Model
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
                ".xaml",
                ".vb",
                ".vbproj",
                ".json",
                ".js",
                ".css",
                ".cshtml"
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
                "\\obj\\"
            });
        }


        public IList<string> ReplaceFiles { get; set; }
        public IList<string> DeleteFiles { get; set; }
        public IList<string> ExcludeFolders { get; set; }
    }
}