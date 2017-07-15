using System;
using System.IO;
using System.Linq;
using Grappachu.SolutionCloner.Engine.Interfaces;
using Grappachu.SolutionCloner.Engine.Model;
using log4net;

namespace Grappachu.SolutionCloner.Engine.Components.Updaters
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

            File.WriteAllText(file.FullName, updated);
        }


        private string RenameFile(FileInfo file, CloneSettings pars)
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