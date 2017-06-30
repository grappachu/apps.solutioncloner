using System;
using System.IO;
using Grappachu.SolutionCloner.Engine.Interfaces;
using Grappachu.SolutionCloner.Engine.Model;
using log4net;

namespace Grappachu.SolutionCloner.Engine.Components
{
    internal class CloneSettingsValidator : IValidator<CloneSettings>
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

            Log.Info("Validation successful");
        }
    }
}