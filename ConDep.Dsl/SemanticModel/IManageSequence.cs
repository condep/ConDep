namespace ConDep.Dsl.SemanticModel
{
    public interface IManageSequence<in T>
    {
        void Add(T operation, bool addFirst = false);
        bool IsValid(Notification notification);
    }
}