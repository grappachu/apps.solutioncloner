using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Grappachu.Apps.SolutionCloner.Engine.Interfaces;
using Grappachu.Apps.SolutionCloner.Engine.Model;
using log4net;

namespace Grappachu.Apps.SolutionCloner.Engine.Components.Updaters
{
    internal class DefaultFileUpdater : IFileUpdater
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DefaultFileUpdater));
        private readonly CloneSettings _settings;

        public DefaultFileUpdater(CloneSettings settings)
        {
            _settings = settings;
        }

        public void Update(FileInfo file)
        {
            if (ShouldUpdate(file, _settings))
            {
                Update(file, _settings);
            }
            RenameFile(file, _settings);
        }


        private bool ShouldUpdate(FileInfo file, CloneSettings pars)
        {
            var fnames = pars.CloneProfile.ReplaceFiles;
            return fnames.Any(f => file.FullName.EndsWith(f, StringComparison.OrdinalIgnoreCase));
        }


        private void Update(FileInfo file, CloneSettings pars)
        {
            var content = File.ReadAllText(file.FullName);

            var updated = content.Replace(pars.TemplateKey, pars.TargetKey);

            // Nel caso di un template applica tutti i replacement definiti
            foreach (var replacement in _settings.CloneProfile.ExtraReplacements)
            {
                updated = replacement.IgnoreCase 
                    ? updated.Replace(replacement.Placeholder, replacement.Value) 
                    : Regex.Replace(updated, replacement.Placeholder, replacement.Value, RegexOptions.IgnoreCase);
            }

            File.WriteAllText(file.FullName, updated);
        }


        private static string RenameFile(FileInfo file, CloneSettings pars)
        {
            var fname = file.Name.Replace(pars.TemplateKey, pars.TargetKey);

            var updatedName = Path.Combine(file.DirectoryName, fname);
            if (file.FullName != updatedName)
            {
                file.MoveTo(updatedName);
                Log.DebugFormat("Renamed as: {0}", updatedName.Replace(pars.TemplateSource.FullName, string.Empty));
                return updatedName;
            }
            return file.FullName;
        }
    }
}