namespace Grappachu.SolutionCloner.Engine.Interfaces
{
    internal interface IValidator<in T>
    {
        void Validate(T toValidate);
    }
}