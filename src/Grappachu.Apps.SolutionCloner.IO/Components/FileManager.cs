using Grappachu.Apps.SolutionCloner.Engine.Interfaces;
using Grappachu.Core.IO;
using Grappachu.Core.Preview.IO;

namespace Grappachu.Apps.SolutionCloner.Engine.Components
{
    public class FileManager : IFileManager
    {
        /// <summary>
        ///     Clones the whole folder <paramref name="sourceFolder" /> into <paramref name="tartgetFolder" />
        /// </summary>
        /// <param name="sourceFolder"></param>
        /// <param name="tartgetFolder"></param>
        public void CloneTree(string sourceFolder, string tartgetFolder)
        {
            PathUtils.Clone(sourceFolder, tartgetFolder);
        }
    }
}