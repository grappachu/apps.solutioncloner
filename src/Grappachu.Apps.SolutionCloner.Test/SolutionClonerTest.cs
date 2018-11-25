using System.IO;
using Grappachu.Apps.SolutionCloner.Engine.Interfaces;
using Grappachu.Apps.SolutionCloner.Engine.Model;
using Moq;
using Xunit;

namespace Grappachu.Apps.SolutionCloner.Test
{
    public class SolutionClonerTest
    {
        private readonly Mock<IFileManager> _fileManager;
        private readonly Mock<IValidator<CloneSettings>> _validator;
        private readonly Mock<IUpdaterFactory<IFileUpdater>> _updateFactory;

        public SolutionClonerTest()
        {
            _updateFactory = new Mock<IUpdaterFactory<IFileUpdater>>();
            _validator = new Mock<IValidator<CloneSettings>>();
            _fileManager = new Mock<IFileManager>();
        }

        [Fact]
        public void Clone()
        {
            var cloneSettings = new CloneSettings()
            {
                TemplateSource = new DirectoryInfo("Source"),
                TargetFolder = new DirectoryInfo("Target")
            };
            var cloner = new Engine.SolutionCloner(_fileManager.Object, _validator.Object, _updateFactory.Object);

            cloner.Clone(cloneSettings);

            _validator.Verify(x => x.Validate(cloneSettings), Times.Once());
            _fileManager.Verify(x => x.CloneTree(cloneSettings.TemplateSource.FullName, cloneSettings.TargetFolder.FullName), Times.Once());


        }
    }
}
