using System;
using System.IO;
using Grappachu.Apps.SolutionCloner.Engine.Interfaces;
using Grappachu.Apps.SolutionCloner.Engine.Model;
using log4net;

namespace Grappachu.Apps.SolutionCloner.Engine.Components
{
    public class CloneSettingsValidator : IValidator<CloneSettings>
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CloneSettingsValidator));

        public void Validate(CloneSettings builderParams)
        {
            if (!builderParams.TemplateSource.Exists)
                throw new DirectoryNotFoundException("Source folder not found");
            if (!builderParams.TargetFolder.Exists)
                throw new DirectoryNotFoundException("Target folder not found");

            if (string.IsNullOrEmpty(builderParams.TemplateKey))
                throw new NullReferenceException("Original namespace is empty");
            if (string.IsNullOrEmpty(builderParams.TargetKey))
                throw new NullReferenceException("Target namespace is empty");

            builderParams.TemplateSource = SanitizeDir(builderParams.TemplateSource);
            builderParams.TargetFolder = SanitizeDir(builderParams.TargetFolder);

            if (builderParams.TargetKey.Contains(builderParams.TemplateKey))
            {
                throw new NotSupportedException("The new namespace cannot contain the original namespace");
            }

            Log.Info("Validation successful");
        }

        private static DirectoryInfo SanitizeDir(DirectoryInfo path)
        {
            var sanitized = path.FullName.Trim('\\', '/');
            return new DirectoryInfo(sanitized);
        }
    }
}