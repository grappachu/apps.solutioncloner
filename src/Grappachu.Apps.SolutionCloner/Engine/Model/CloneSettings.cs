using System.IO;

namespace Grappachu.SolutionCloner.Engine.Model
{
    internal class CloneSettings
    {
        public CloneSettings()
        {
            CloneProfile = new CloneProfile();
        }

        public DirectoryInfo TargetFolder { get; set; }
        public DirectoryInfo TemplateSource { get; set; }

        public string TemplateKey { get; set; }

        public string TargetKey { get; set; }

        public CloneProfile CloneProfile { get; }
    }
}