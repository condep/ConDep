namespace ConDep.Dsl.SemanticModel.Sequence
{
    public interface IManageSequence<in T>
    {
        void Add(T operation, bool addFirst = false);
        bool IsValid(Notification notification);
    }
}