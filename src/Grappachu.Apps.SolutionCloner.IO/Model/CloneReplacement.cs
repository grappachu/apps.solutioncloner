namespace Grappachu.Apps.SolutionCloner.Engine.Model
{
    public class CloneReplacement
    {
        public string Placeholder { get; set; }
        public string Value { get; set; }
        public bool IgnoreCase { get; set; }
        public string Description { get;  set; }
        public bool IsMandatory { get; set; }
    }
}