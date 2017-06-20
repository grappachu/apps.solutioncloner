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
            if (!builderParams.TargetFolder.Exists)
                throw new DirectoryNotFoundException("Target folder not found");
            Log.Info("Validation successful");
        }
    }
}