using System.IO;
using Grappachu.SolutionCloner.Engine.Components;
using Grappachu.SolutionCloner.Engine.Model;

namespace Grappachu.SolutionCloner.Engine.Interfaces
{
    internal interface IUpdaterFactory<T>
    {
        IFileUpdater CreateUpdater(FileInfo file, CloneSettings settings);
    }
}