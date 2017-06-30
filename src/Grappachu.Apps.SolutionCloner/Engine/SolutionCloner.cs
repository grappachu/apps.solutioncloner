using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Grappachu.Core.Preview.IO;
using Grappachu.SolutionCloner.Engine.Components;
using Grappachu.SolutionCloner.Engine.Interfaces;
using Grappachu.SolutionCloner.Engine.Model;
using log4net;

namespace Grappachu.SolutionCloner.Engine
{
    internal class SolutionCloner
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SolutionCloner));
        private readonly IUpdaterFactory<IFileUpdater> _updaterFactory;
        private readonly IValidator<CloneSettings> _validator;


        public SolutionCloner(IValidator<CloneSettings> validator, IUpdaterFactory<IFileUpdater> updaterFactory)
        {
            _validator = validator;
            _updaterFactory = updaterFactory;
        }

        public void Clone(CloneSettings cloneSettings)
        {
            try
            {
                Log.Info("Validating parameters...");
                _validator.Validate(cloneSettings);

                Log.Info("Cloning source tree into new folder...");
                CloneSourceTree(cloneSettings);

                //Log.Info("Removing Source Control References...");
                //_sccRemover.Clean(cloneSettings);

                Log.Info("Removing unwanted files...");
                CleanUnwantedFiles(cloneSettings);

                Log.Info("Updating files...");
                UpdateFiles(cloneSettings);

                Log.Info("Rebuilding solution tree...");
                RebuildDirectoryTree(cloneSettings);

                Log.Info("Completed!");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }


        private void RebuildDirectoryTree(CloneSettings cloneSettings)
        {
            var root = cloneSettings.TargetFolder;
            RenameDirectories(cloneSettings, root);
        }

        private void UpdateFiles(CloneSettings cloneSettings)
        {
            var allFiles = cloneSettings.TargetFolder.GetFiles("*", SearchOption.AllDirectories).ToArray();
            foreach (var file in allFiles)
            {
                try
                {
                    var updater = _updaterFactory.CreateUpdater(file, cloneSettings);
                    if (updater != null)
                    {
                        var relativePath = AsRelative(cloneSettings, file);
                        updater.Update(file);
                        Log.DebugFormat("Updated: {0}", relativePath);
                    }
                }
                catch (UnauthorizedAccessException uaex)
                {
                    Log.WarnFormat("Cannot delete: {0} [{1}]", file, uaex.Message);
                }
            }
        }

        private static string AsRelative(CloneSettings cloneSettings, FileInfo file)
        {
            var relativePath = file.FullName.Replace(cloneSettings.TargetFolder.FullName, string.Empty);
            return relativePath;
        }

        private static void CloneSourceTree(CloneSettings cloneSettings)
        {
            FilesystemTools.ClonePath(cloneSettings.TemplateSource.FullName, cloneSettings.TargetFolder.FullName);
            Log.Debug("All files copied to target path");
        }

        private void CleanUnwantedFiles(CloneSettings cloneSettings)
        {
            var allFiles = cloneSettings.TargetFolder.GetFiles("*", SearchOption.AllDirectories).ToArray();
            foreach (var file in allFiles)
            {
                try
                {
                    if (ShouldDelete(file, cloneSettings))
                    {
                        var relativePath = AsRelative(cloneSettings, file);
                        file.Delete();
                        Log.DebugFormat("Deleted: {0}", relativePath);
                    }
                }
                catch (UnauthorizedAccessException uaex)
                {
                    Log.WarnFormat("Cannot delete: {0} [{1}]", file, uaex.Message);
                }
            }
        }


        private void RenameDirectories(CloneSettings builderParams, DirectoryInfo root)
        {
            var dirs = root.GetDirectories("*").ToArray();
            foreach (var dir in dirs)
            {
                var renamedRoot = Rename(dir, builderParams);

                RenameDirectories(builderParams, new DirectoryInfo(renamedRoot));
            }
        }


        private string Rename(DirectoryInfo dir, CloneSettings pars)
        {
            var updatedName = dir.FullName.Replace(pars.TemplateKey, pars.TargetKey);
            if (dir.FullName != updatedName)
            {
                var srcDir = dir.FullName;
                dir.MoveTo(updatedName);
                Log.DebugFormat("Direcotry Moved: {0}\n\t ==> {1}", srcDir, updatedName);

                return updatedName;
            }
            return dir.FullName;
        }


        private static bool ShouldDelete(FileInfo file, CloneSettings pars)
        {
            var fpath = file.FullName.ToLowerInvariant();
            return pars.CloneProfile.ExcludeFolders.Any(f => fpath.Contains(f.ToLowerInvariant()))
                || pars.CloneProfile.DeleteFiles.Any(f => fpath.EndsWith(f, StringComparison.OrdinalIgnoreCase));
        }
    }
}