using System.Collections.Generic;
using System.IO;

namespace Grappachu.Apps.SolutionCloner.Engine.Model.Templates
{
    public class TemplateInfo
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public Author Author { get; set; }
        public string Description { get; set; }
        public string Namespace { get; set; }
        public List<Placeholder> Placeholders { get; set; }
        public List<SolutionItemReference> SolutionItems { get; set; }
        public string TemplatePath { get; set; }
        public string DataPath { get; set; }


        public override string ToString()
        {
            return string.Format("{0} v.{1}", Name, Version);
        }

        public string GetCloneableRoot()
        {
            if (!Path.IsPathRooted(DataPath))
            {
                var startPath = Path.GetDirectoryName(TemplatePath);
                var dirPath = Path.Combine(startPath, DataPath);
                return dirPath;
            }

            return DataPath;
        }
    }
}