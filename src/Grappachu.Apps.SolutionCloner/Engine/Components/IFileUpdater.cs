using System.IO;

namespace Grappachu.SolutionCloner.Engine.Components
{
    internal interface IFileUpdater
    {
        void Update(FileInfo file);
    }
}