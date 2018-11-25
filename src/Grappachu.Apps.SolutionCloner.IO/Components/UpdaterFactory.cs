using System.IO;
using Grappachu.Apps.SolutionCloner.Engine.Components.Updaters;
using Grappachu.Apps.SolutionCloner.Engine.Interfaces;
using Grappachu.Apps.SolutionCloner.Engine.Model;

namespace Grappachu.Apps.SolutionCloner.Engine.Components
{
    public class UpdaterFactory<T> : IUpdaterFactory<T>
    {
        public IFileUpdater CreateUpdater(FileInfo file, CloneSettings settings)
        {
           return  new DefaultFileUpdater(settings);
        }
    }
}