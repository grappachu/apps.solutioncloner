using System.Collections.Generic;
using Grappachu.Apps.SolutionCloner.Engine.Model.Templates;

namespace Grappachu.Apps.SolutionCloner.Engine.Interfaces
{
    public interface ITemplateEnumerator
    {
        IReadOnlyCollection<TemplateInfo> GetTemplates();
    }
}