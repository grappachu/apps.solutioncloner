using System;
using Grappachu.SolutionCloner.Properties;

namespace Grappachu.SolutionCloner
{
    public class ReadConfig
    {
        public static string TemplateFolder
        {
            get => Environment.ExpandEnvironmentVariables(Settings.Default.TemplateFolder);
        }
    }
}