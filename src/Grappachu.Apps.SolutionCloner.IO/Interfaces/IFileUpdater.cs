using System.IO;

namespace Grappachu.Apps.SolutionCloner.Engine.Interfaces
{
    public interface IFileUpdater
    {
        void Update(FileInfo file);
    }
}