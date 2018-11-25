using System.IO;
using Grappachu.Apps.SolutionCloner.Engine.Model;

namespace Grappachu.Apps.SolutionCloner.Engine.Interfaces
{
    public interface IUpdaterFactory<T>
    {
        IFileUpdater CreateUpdater(FileInfo file, CloneSettings settings);
    }
}