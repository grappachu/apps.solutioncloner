using System;
using System.IO;
using System.IO.Compression;
using Grappachu.Core.IO;
using Grappachu.Core.Preview.IO;
using Grappachu.SolutionCloner.Properties;

namespace Grappachu.SolutionCloner
{
    public class TemplateManager
    {
        private readonly string _templateFolder;

        public TemplateManager()
        {
            _templateFolder = ReadConfig.TemplateFolder;
            FilesystemTools.SafeCreateDirectory(_templateFolder);

        }

        public void BuildTemplates()
        {
            UnzipTemplate(Resources.NugetLibrary_1_0_0, Path.Combine(_templateFolder, "NugetLibrary.1.0.0"));
        }

        /// <summary>
        ///     Extract the whole archive in the folder
        /// </summary> 
        /// <param name="templateData"></param>
        /// <param name="targetFolder"></param>
        private static void UnzipTemplate(byte[] templateData, string targetFolder)
        {
            using (var tmp = new TempFile(".zip"))
            {
                File.WriteAllBytes(tmp.Path, templateData);
                FilesystemTools.SafeDelete(targetFolder, true, true);
                ZipFile.ExtractToDirectory(tmp.Path, targetFolder);
            }
        }
    }
}