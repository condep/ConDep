namespace ConDep.Console
{
    public abstract class CmdBaseValidator<T>
    {
        public abstract void Validate(T options);
    }
}