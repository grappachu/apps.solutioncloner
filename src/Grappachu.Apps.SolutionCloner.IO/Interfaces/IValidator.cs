namespace Grappachu.Apps.SolutionCloner.Engine.Interfaces
{
    public interface IValidator<in T>
    {
        void Validate(T toValidate);
    }
}