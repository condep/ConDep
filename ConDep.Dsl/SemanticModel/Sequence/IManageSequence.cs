namespace ConDep.Dsl.SemanticModel.Sequence
{
    public interface IManageSequence<in T>
    {
        void Add(T operation);
    }
}