namespace ConDep.Dsl.SemanticModel.Sequence
{
    public interface ISequenceElement : IValidate
    {
        IReportStatus Execute(IReportStatus status);
    }
}