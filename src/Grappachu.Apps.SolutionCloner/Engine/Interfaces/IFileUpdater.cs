using System.IO;

namespace Grappachu.SolutionCloner.Engine.Interfaces
{
    internal interface IFileUpdater
    {
        void Update(FileInfo file);
    }
}