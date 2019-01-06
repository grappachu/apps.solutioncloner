using Grappachu.Core.Lang.Extensions;

namespace Grappachu.Apps.SolutionCloner.Engine.Model.Templates
{
    public class Author
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Url { get; set; }

        public override string ToString()
        {
            return FullName.Or(Name);
        }
    }
}