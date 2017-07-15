using System.IO;
using Grappachu.SolutionCloner.Engine.Components.Updaters;
using Grappachu.SolutionCloner.Engine.Interfaces;
using Grappachu.SolutionCloner.Engine.Model;

namespace Grappachu.SolutionCloner.Engine.Components
{
    class UpdaterFactory<T> : IUpdaterFactory<T>
    {
        public IFileUpdater CreateUpdater(FileInfo file, CloneSettings settings)
        {
           return  new DefaultFileUpdater(settings);
        }
    }
}